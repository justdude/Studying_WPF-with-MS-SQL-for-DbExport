
//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace DbExport.MVVM.Catel_4._0._0.Catel.Test.Catel.Test.EntityFramework5.DbContextTest.NET45
{

using System;
    using System.Collections.Generic;
    
public partial class DbContextOrder
{

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public System.DateTime OrderCreated { get; set; }

    public int Amount { get; set; }



    public virtual DbContextCustomer DbContextCustomer { get; set; }

    public virtual DbContextProduct DbContextProduct { get; set; }

}

}
