using Inventory.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace Inventory.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;
        public ProductService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entities.Product>> Get()
        {
            return await _context.Products.ToListAsync();
        }

        
        public async Task<Entities.Product> GetOne(int id)
        {
            var isExist = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (isExist == null)
            {
                return new Entities.Product();
            }
            return isExist;
        }

        public async Task<int> Add(Entities.Product product)
        {
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
            return product.Id;
        }

        public async Task<IEnumerable<TEntity>> LoadAllWithRelatedAsync<TEntity>(params Expression<Func<TEntity, object>>[] expressionList) where TEntity : class
        {
            var entities = _context.Set<TEntity>();
            var query = entities.AsQueryable();
            foreach (var expression in expressionList)
            {
                query = query.Include(expression);
            }

            return await query.ToListAsync();
        }
        public void SaveTableRecord<TEntity>(List<DynamicField> fields, Dictionary<string, object> fieldValues) where TEntity : class
        {
            try
            {
                TEntity poco = Activator.CreateInstance<TEntity>();
                string pkName = fields.Where(x => x.IsPrimaryKey).Select(x => x.ColumnName).FirstOrDefault();
                // bool hasAutoID = false;
                foreach (var f in fields.Where(x => !(x.IsGroup.HasValue && x.IsGroup.Value)).ToList())
                {
                    #region Set Values (This Logic can be enhanced by Wraping Around Like PetaPococ)
                    if (f.IsAutoID)
                    {
                        //hasAutoID = true;
                        continue;
                    }
                    if (poco.GetType().GetProperty(f.Name) == null)
                    {
                        continue;
                    }
                    if (!fieldValues.ContainsKey(f.Name))
                    {
                        poco.GetType().GetProperty(f.Name).SetValue(poco, null);
                        continue;
                    }
                    if (f.IsBoolean && !string.IsNullOrEmpty(fieldValues[f.Name].ToString()))
                    {
                        poco.GetType().GetProperty(f.Name).SetValue(poco, bool.Parse(fieldValues[f.Name].ToString()));
                    }
                    else if (f.IsInt16 && !string.IsNullOrEmpty(fieldValues[f.Name].ToString()))
                    {
                        poco.GetType().GetProperty(f.Name).SetValue(poco, Int16.Parse(fieldValues[f.Name].ToString()));
                    }
                    else if (f.IsInt32 && !string.IsNullOrEmpty(fieldValues[f.Name].ToString()))
                    {
                        poco.GetType().GetProperty(f.Name).SetValue(poco, Int32.Parse(fieldValues[f.Name].ToString()));
                    }
                    else if (f.IsDateTime && !string.IsNullOrEmpty(fieldValues[f.Name].ToString()))
                    {
                        poco.GetType().GetProperty(f.Name).SetValue(poco, DateTime.Parse(fieldValues[f.Name].ToString()));
                    }
                    else if (f.IsFloat)
                    {
                        poco.GetType().GetProperty(f.Name).SetValue(poco, double.Parse(fieldValues[f.Name].ToString()));
                    }
                    else if (!string.IsNullOrEmpty(fieldValues[f.Name].ToString()))
                    {
                        poco.GetType().GetProperty(f.Name).SetValue(poco, fieldValues[f.Name]);


                        //if (poco.GetType().GetProperty(f.Name).PropertyType.GetCoreType().Name.ToLower().Contains("decimal"))
                        //{
                        //    poco.GetType().GetProperty(f.Name).SetValue(poco, decimal.Parse(fieldValues[f.Name].ToString().Trim()));
                        //}
                        //else
                        //{
                        //    poco.GetType().GetProperty(f.Name).SetValue(poco, fieldValues[f.Name].ToString().Trim());
                        //}
                    }
                    #endregion
                }
                _context.Set<TEntity>().Add(poco);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public partial class EntityDetail
    {


        [Column]
        public int EntityDetailsID { get; set; }

        [Column]
        public short EntityID { get; set; }

        [Column]
        public string ColumnName { get; set; }

        [Column]
        public string DisplayNameEn { get; set; }

        [Column]
        public string DisplayNameAr { get; set; }

        [Column]
        public short DbTypeID { get; set; }

        [Column]
        public bool IsRequired { get; set; }

        [Column]
        public bool? IsGroup { get; set; }

        [Column]
        public int? ParentSubGroupID { get; set; }

        [Column]
        public int? ParentEntityDetailsID { get; set; }

        [Column]
        public bool IsForeignkey { get; set; }

        [Column]
        public string RefrencedTableName { get; set; }

        [Column]
        public string RefrencedColumnName { get; set; }

        [Column]
        public bool EnableAutoComplate { get; set; }

        [Column]
        public string AutoCompleteSourceQuery { get; set; }

        [Column]
        public bool? IsDigit { get; set; }

        [Column]
        public string ValidationExpression { get; set; }

        [Column]
        public bool AddEditVisible { get; set; }

        [Column]
        public bool GridVisible { get; set; }

        [Column]
        public bool SearchFilterVisible { get; set; }

        [Column]
        public short MaxLength { get; set; }

        [Column]
        public bool IsAutoID { get; set; }

        [Column]
        public bool IsPrimaryKey { get; set; }

        [Column]
        public int? DisplaySeqNo { get; set; }

        [Column]
        public bool? IsFileUpload { get; set; }

        [Column]
        public string AllowedFiles { get; set; }

        [Column]
        public bool ShowGroupTitle { get; set; }

        [Column]
        public string PlaceHolderText { get; set; }

        [Column]
        public string DisplayFormat { get; set; }
    }

    public class DynamicField : EntityDetail
    {
        Dictionary<int, string> dic;
        public DynamicField()
        {
            dic = new Dictionary<int, string>();
            dic.Add(35, "String");
            dic.Add(36, "Guid");
            dic.Add(40, "DateTime");
            dic.Add(41, "DateTime");
            dic.Add(42, "DateTime");
            dic.Add(58, "DateTime");
            dic.Add(61, "DateTime");
            dic.Add(99, "String");
            dic.Add(167, "String");
            dic.Add(175, "String");
            dic.Add(239, "String");
            dic.Add(231, "String");
            dic.Add(48, "Short");
            dic.Add(52, "Int16");
            dic.Add(56, "Int32");
            dic.Add(127, "Int64");
            dic.Add(106, "Decimal");
            dic.Add(108, "Decimal");
            dic.Add(122, "Decimal");
            dic.Add(60, "Decimal");
            dic.Add(62, "Float");
            dic.Add(104, "Boolean");
            this.Disaplayble = true;
            this.AllowInGrid = true;
            this.GridOrder = 256;
        }
        public string Name
        {
            get
            {
                return this.ColumnName;
            }
            set
            {
                this.ColumnName = value;
            }
        }
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DisplayNameEn))
                {
                    if (this.Name == this.DisplayNameEn)
                    {
                        return this.DisplayNameEn;
                    }
                    return this.DisplayNameEn;
                }
                return this.Name;
            }
        }
        //[ResultColumn]
        //public bool AllowNulls 
        //{
        //    get { return this.IsRequired; }
        //    set { this.IsRequired = !value; }
        //}
        public int ProperMaxLength
        {
            get
            {
                if (this.IsUniCode)
                {
                    return this.MaxLength / 2;
                }
                return this.MaxLength;
            }
        }
        public bool Disaplayble { get; set; }

        public string ValueColumnName { get; set; }
        public string TextColumnName { get; set; }
        public bool AllowInGrid { get; set; }
        public int GridOrder { get; set; }
        public List<Dictionary<string, object>> RefrencedTableData { get; set; }
        public string FieldType
        {
            get { return null; }
        }
        public bool IsBoolean
        {
            get
            {
                return (this.FieldType == "Boolean");
            }
        }
        public bool IsFloat
        {
            get
            {
                return (this.FieldType == "Float");
            }
        }
        public bool IsString
        {
            get
            {
                return (this.FieldType == "String");
            }
        }
        public bool IsInt16
        {
            get
            {
                return (this.FieldType == "Int16");
            }
        }
        public bool IsInt32
        {
            get
            {
                return (this.FieldType == "Int32");
            }
        }
        public bool IsInt64
        {
            get
            {
                return (this.FieldType == "Int64");
            }
        }
        public bool IsDecimal
        {
            get
            {
                return (this.FieldType == "Decimal");
            }
        }
        public bool IsDateTime
        {
            get
            {
                return (this.FieldType == "DateTime");
            }
        }
        public bool IsUniCode
        {
            get
            {
                return (true);
            }
        }

    }
}
