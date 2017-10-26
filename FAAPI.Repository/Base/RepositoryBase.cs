
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using Dommel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using static Dommel.DommelMapper;

namespace FAAPI.Repository.Base
{

    public abstract class RepositoryBase<TModel> where TModel : class
    {
        protected String Cs { get; }

        private Type type = typeof(TModel);
        private string tableName;
        private PropertyInfo keyProperty;
        private List<PropertyInfo> typeProperties;
        private string[] columnNames;
        private string[] paramNames;

        public string GetTableName()
        {
            return tableName;
        }
        public string[] GetColumnNames()
        {
            return columnNames;
        }
        public string[] GetParamNames()
        {
            return paramNames;
        }
        protected RepositoryBase()
        {
            Cs = GetConnString();
            //var db = new SqlConnection(Cs);
            //db.Open();
            tableName = Resolvers.Table(type);
            try {
                keyProperty = Resolvers.KeyProperty(type);
            }
            catch
            {
                keyProperty = null;
            }
            
            typeProperties = ResolveProperties(type).Where(p => p != keyProperty).ToList();
            columnNames = typeProperties.Select(Resolvers.Column).ToArray();
            paramNames = typeProperties.Select(p => "@" + p.Name).ToArray();
            //db.Close();
            
        }

        public virtual TModel GetById(object id)
        {
            using (var db = new SqlConnection(Cs))
            {
                return db.Get<TModel>(id);
            }
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            using (var db = new SqlConnection(Cs))
            {
                return db.GetAll<TModel>();
            }
        }

        public virtual IEnumerable<TModel> GetByIds(List<int> ids)
        {
            string sql = string.Format("SELECT * FROM {0} where {1} IN @ids",
                tableName,
                Resolvers.Column(keyProperty)
            );

            using (var db = new SqlConnection(Cs))
            {
                return db.Query<TModel>(sql, new { ids });
            }
            
        }

        public virtual IEnumerable<TModel> GetByIds(List<Guid> ids)
        {
            string sql = string.Format("SELECT * FROM {0} where {1} IN @ids",
                tableName,
                Resolvers.Column(keyProperty)
            );

            using (var db = new SqlConnection(Cs))
            {
                return db.Query<TModel>(sql, new { ids });
            }

        }

        public virtual void Add(TModel model)
        {
            using (var db = new SqlConnection(Cs))
            {
                if (keyProperty != null)
                {
                    var key = DommelMapper.Resolvers.KeyProperty(typeof(TModel));

                    if (key.PropertyType == typeof(Guid))
                    {
                        var id = Insert(model);
                        key.SetValue(model, id, null);
                    }
                    else
                    {
                        var id = db.Insert(model);
                        key.SetValue(model, id, null);
                    }
                }
                else
                {
                    InsertSimple(model);
                }
            }
        }

        public string BuildInsertPkGuid(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty)
        {
            string _pk = DommelMapper.Resolvers.Column(keyProperty);
            string str = "DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER) " +
            "set nocount on INSERT INTO {0} ({1}) " +
            "OUTPUT INSERTED.{2} INTO @myNewPKTable " +
            "VALUES ({3}) " +
            "SELECT * FROM @myNewPKTable";

            return string.Format(str,
                tableName,
                string.Join(", ", columnNames),
                 _pk,
                string.Join(", ", paramNames));
        }

        public string BuildInsertSimple(string tableName, string[] columnNames, string[] paramNames)
        {
            //string _pk = DommelMapper.Resolvers.Column(keyProperty);
            string str = "INSERT INTO {0} ({1}) VALUES ({2}) ";

            return string.Format(str,
                tableName,
                string.Join(", ", columnNames),
                string.Join(", ", paramNames));
        }

        public string BuildSearch(string tableName, string[] columnNames, string[] paramNames, object searchProperties)
        {
            string str = "SELECT * FROM {0} ";

            List<string> where = new List<string>();
            int n = 0;
            foreach (var property in paramNames)
            {
                foreach (var kv in (IDictionary<string, object>)searchProperties)
                {
                    if ("@" + kv.Key == property)
                    {
                        if ((type.GetProperty(kv.Key).PropertyType == typeof(String)))
                        {
                            where.Add(columnNames[n] + " LIKE @" + kv.Key);
                        }
                        else if (kv.Value.GetType() == typeof(List<int>))
                        {
                            where.Add(columnNames[n] + " IN @" + kv.Key);
                        }
                        else if (kv.Value.GetType() == typeof(List<Guid>))
                        {
                            where.Add(columnNames[n] + " IN @" + kv.Key);
                        }
                        else
                        {
                            where.Add(columnNames[n] + " = @" + kv.Key);
                        }
                    }
                    else if ("@" + kv.Key == property + "De")
                    {
                        where.Add(columnNames[n] + " >= @" + kv.Key);
                    }
                    else if ("@" + kv.Key == property + "Ate")
                    {
                        where.Add(columnNames[n] + " <= @" + kv.Key);
                    }
                }
                n++;
            }

            foreach (var kv in (IDictionary<string, object>)searchProperties)
            {
                if (keyProperty != null && kv.Key == keyProperty.Name && (kv.Value.GetType() == typeof(List<int>) || kv.Value.GetType() == typeof(List<Guid>)))
                {
                    where.Add(Resolvers.Column(keyProperty) + " IN @" + kv.Key);
                }
            }

            if (where.Count > 0)
            {
                str += " WHERE {1}";
            }

            return string.Format(str, tableName, string.Join(" AND ", where));
        }

        public List<PropertyInfo> ResolveProperties(Type type)
        {
            IEntityMap entityMap;
            List<PropertyInfo> props = new List<PropertyInfo>();
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                foreach (var property in type.GetProperties())
                {
                    var propertyMap = entityMap.PropertyMaps.FirstOrDefault(p => p.PropertyInfo.Name == property.Name);
                    if (propertyMap != null && !propertyMap.Ignored)
                    {
                        props.Add(property);
                    }
                }
            }
            return props;
        }

        public Guid Insert(TModel model)
        {
            using (var db = new SqlConnection(Cs))
            {
                //var type = typeof(TModel);

                string sql;
                sql = BuildInsertPkGuid(tableName, columnNames, paramNames, keyProperty);
                var result = db.Query<Guid>(sql, model);
                return result.Single();
            }
        }

        public void InsertSimple(TModel model)
        {
            using (var db = new SqlConnection(Cs))
            {
                //var type = typeof(TModel);

                string sql;
                sql = BuildInsertSimple(tableName, columnNames, paramNames);
                db.Query<Guid>(sql, model);
                
            }
        }

        public virtual bool Update(TModel model)
        {
            using (var db = new SqlConnection(Cs))
            {
                //var type = typeof(TModel);
                var columnNamesUp = columnNames;
                columnNamesUp = typeProperties.Select(p => string.Format("{0} = @{1}", Resolvers.Column(p), p.Name)).ToArray();
                string sql;
                sql = string.Format("update {0} set {1} where {2} = @{3}",
                    tableName,
                    string.Join(", ", columnNamesUp),
                    Resolvers.Column(keyProperty),
                    keyProperty.Name);

                return db.Execute(sql,model) > 0;
            }
        }

        [Obsolete("Esse metódo faz a exclusão 'física' do dado na tabela")]
        public virtual bool Delete(object id)
        {
            using (var db = new SqlConnection(Cs))
            {
                var obj = db.Get<TModel>(id);

                return db.Delete(obj);
            }
        }

        public virtual IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> predicate)
        {
            using (var db = new SqlConnection(Cs))
            {
                return db.Select<TModel>(predicate);
            }
        }

        public virtual TModel GetOne(Expression<Func<TModel, bool>> predicate)
        {
            using (var db = new SqlConnection(Cs))
            {
                var result = db.Select<TModel>(predicate);
                if(result.Count() > 0)
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return null;
                }
                
            }
        }

        public virtual IEnumerable<TModel> GetList(Object searchProperties)
        {
            using (var db = new SqlConnection(Cs))
            {
                string sql;
                sql = BuildSearch(tableName, columnNames, paramNames, searchProperties);
                return db.Query<TModel>(sql, searchProperties);
            }
        }



        public virtual decimal getTotal()
        {
            using (var db = new SqlConnection(Cs))
            {
                string sql = string.Format(@"SELECT COUNT(0) FROM {0}", Resolvers.Table(typeof(TModel)));
                var result = db.Query<int>(sql);
                return result.Single();
            }
        }
        public virtual decimal getTotal(Expression<Func<TModel, bool>> predicate)
        {
            return GetList(predicate).Count();
        }

        private String GetConnString()
        {
           
            return "Server=.;DataBase=FADB;uid=faapi;pwd=f44p1;Min Pool Size=5;Max Pool Size=250;MultipleActiveResultSets=true";

        }

    }
}
