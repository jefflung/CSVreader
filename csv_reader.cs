using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Business Objects layer
/// - Contains implementation of classes
/// - Doesn’t reference any other layers
/// </summary>

namespace BusinessObjects
{

    /// <summary>
    /// Product
    /// </summary>

    public class Product
    {

        /// <summary>
        /// Product ID
        /// </summary>

        public string ID { get; set; }
    
        /// <summary>
        /// Product Description
        /// </summary>

        public string Description { get; set; }
    }
}   

/// <summary>
/// Data Access layer
/// - Contains means of connecting to data sources and retrieving/updating data
/// - Doesn’t reference any other layers
/// </summary>

namespace DataAccess
{

    /// <summary>
    /// Provides facilities to retrieve, add, update and delete data from a database.
    /// </summary>

    public class DatabaseManager
    {

        /// <summary>
        /// Gets single instance of object of type T from the database.
        /// </summary>
        /// <param name="query">Query used to query the database</param>
        /// <param name="parameters">Parameters to be used in the query</param>
        /// <returns>Instance of object of type T</returns>
        
        public static T GetSingleInstance<T>(string query, object[] parameters)
        {
            T instance = default(T);
            
            //Implementation here
            //...

            

            return instance (
                "SELECT * FROM [dbo].[Products] WITH(NOLOCK) WHERE [QUERY] = @QUERY;", new object[] { "@QUERY", query };
            );
        }
    }
}

/// <summary>
/// Models layer
/// - Contains facilities using Data Access layer to retrieve/update data
/// represented as instances of classes stored in the Business Objects layer
/// - References Business Objects and Data Access layers
/// </summary>

namespace Models
{

    /// <summary>
    /// Provides facilities to manage Products.
    /// </summary>

    public class ProductModel
    {

        /// <summary>
        /// Gets Product from the database.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product</returns>
        
        public static BusinessObjects.Product GetProduct(string id)
        {
            return DataAccess.DatabaseManager.GetSingleInstance<BusinessObjects.Product>(
                "SELECT * FROM [dbo].[Products] WITH(NOLOCK) WHERE [ID] = @ID;", new object[] { "@ID", id }
            );
        }
    }
}

/// <summary>
/// Views layer
/// - Contains views used by presenters to capture user actions and display data
/// - References Business Objects layer
/// </summary>

namespace Views
{

    /// <summary>
    /// Definition of Product View.
    /// </summary>
    
    public interface IProductView
    {

        /// <summary>
        /// Adds or removes event called when 'Get Product Description' button is clicked.
        /// </summary>

        event EventHandler GetProductDescriptionButtonClick;
        
        /// <summary>
        /// Gets Product ID entered by User.
        /// </summary>
        
        string ProductID { get; }
        
        /// <summary>
        /// Sets Product Description.
        /// </summary>
        
        string ProductDescription { set; }
        
        /// <summary>
        /// Shows View.
        /// </summary>
        
        void ShowView();
    }

    /// <summary>
    /// Implementation of Product View.
    /// </summary>
    
    public class ProductView : System.Windows.Forms.Form, Views.IProductView
    {
        //Form controls
        private System.Windows.Forms.TextBox productIdTextBox = null;
        private System.Windows.Forms.Label productDescriptionLabel = null;
        private System.Windows.Forms.Button getProductDescriptionButton = null;
       
        /// <summary>
        /// Adds or removes event called when 'Get Product Description' button is clicked.
        /// </summary>
       
        public event EventHandler GetProductDescriptionButtonClick
        {
            add { getProductDescriptionButton.Click += value; }
            remove { getProductDescriptionButton.Click -= value; }
        }
       
        /// <summary>
        /// Gets Product ID entered by User.
        /// </summary>
       
        public string ProductID
        {
            get { return productIdTextBox.Text; }
        }
       
        /// <summary>
        /// Sets Product Description.
        /// </summary>
       
        public string ProductDescription
        {
            set { productDescriptionLabel.Text = value; }
        }
       
        /// <summary>
        /// Shows View.
        /// </summary>
       
        public void ShowView()
        {
            this.ShowDialog();
        }
    }
}

/// <summary>
/// Presenters layer
/// - Contains presenters acting upon data from the Model and presenting it
/// to the user via Views
/// - References Business Objects, Model and View layers
/// </summary>

namespace Presenters
{
    /// <summary>
    /// Implementation of Product Presenter.
    /// </summary>

    public class ProductPresenter
    {
        //View

        private Views.IProductView _view = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view">View</param>

        public ProductPresenter(Views.IProductView view)
        {
            _view = view;
            //Assign View events
            _view.GetProductDescriptionButtonClick += (x, y) => GetProductDescription();
            //Show View
            _view.ShowView();
        }

        /// <summary>
        /// Gets Product from the database using Product ID entered by user
        /// and updates View with Product Description.
        /// </summary>

        private void GetProductDescription()
        {
            _view.ProductDescription = Models.ProductModel.GetProduct(_view.ProductID).Description;
        }
    }
}