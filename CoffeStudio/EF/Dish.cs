//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoffeStudio.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dish
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dish()
        {
            this.OrderDish = new HashSet<OrderDish>();
        }
    
        public int IDDish { get; set; }
        public string DishTitle { get; set; }
        public decimal Cost { get; set; }
        public int IDCategory { get; set; }
        public Nullable<int> WeightInGrams { get; set; }
        public Nullable<int> VolumeInMililitres { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> Remains { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Category Category1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDish> OrderDish { get; set; }
    }
}