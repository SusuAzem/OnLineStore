using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net;
using Core;

namespace OnLineStore.ViewModels
{

    /// <summary>
    /// For the ECommerce index page
    /// </summary>
    public class ProductListViewModel
    {
        public IEnumerable<Product>? Products { get; set; }

        public IEnumerable<ProductType>? Types { get; set; }

    }
}
