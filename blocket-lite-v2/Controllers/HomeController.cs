using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using blocket_lite.Models;
using blocket_lite.Models.ProductViewModel;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;

//1', '2', '3','4','5','6','7'); DELETE FROM products3 WHERE price='0'; --
namespace blocket_lite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //public static string filter;
    //public static string filterToUser;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    //public RedirectResult Show_clothes()
    //{
    //    filter = "cloths";
    //    return Redirect("https://localhost:7175/Home/Index");
    //}
    //public RedirectResult Show_vehicles()
    //{
    //    filter = "vehicles";
    //    return Redirect("https://localhost:7175/Home/Index");
    //}
    //public RedirectResult Show_all()
    //{
    //    filter = "all";
    //    return Redirect("https://localhost:7175/Home/Index");
    //}

    public IActionResult Index(string filter,string orderVal)
    {
        // hämtar alla objekt i databasen
        var order = Order(orderVal);
        var itemListModel = GetAllItems(filter,order);
        return View(itemListModel);
    }

    public IActionResult AddNewProduct()
    {
        return View();
    }

    public IActionResult MyProducts()
    {
        var userItemList = GetUserItems();
        return View(userItemList);
    }
    public IActionResult LikedByUser()
    {
        var userLikedItems = GetUserLikedItems();
        return View(userLikedItems);
    }
    public IActionResult UserCart()
    {

        var userCartItems = GetUserCartItems();
        return View(userCartItems);
    }
    public IActionResult CheckOut()
    {
        var userCartItems = GetUserCartItems();
        return View(userCartItems);
    }
    public IActionResult OrderPage()
    {
        var userCartItems = GetUserCartItems();
        return View(userCartItems);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    internal string Order(string orderVal)
    {
        switch (orderVal)
        {
            case "PriceUp":
                    return "price";
                break;

            case "PriceDown":
                return "price DESC";
                break;
            case "AÖ":
                return "title";
                break;
            case "ÖA":
                return "title DESC";
                break;
            default:
                return "price";
                break;
        }
        
    }

    //Lägger in values från formulär till databasen.
    public RedirectResult Insert(ItemModel product)
    {

        // if (product.category == "vehicle")
        // {
        using (SqliteConnection con =
       new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                string txtSQL = "INSERT INTO products4 (category,title,price,description,image,miles,year,color,username) VALUES (@0,@1,@2,@3,@4,@5,@6,@7,@8)";


                con.Open();

                tableCmd.CommandText = txtSQL;

                tableCmd.Parameters.AddWithValue("@0", product.category);
                tableCmd.Parameters.AddWithValue("@1", product.title);
                tableCmd.Parameters.AddWithValue("@2", product.price);
                tableCmd.Parameters.AddWithValue("@3", product.description);
                tableCmd.Parameters.AddWithValue("@4", product.image);
                tableCmd.Parameters.AddWithValue("@5", product.miles);
                tableCmd.Parameters.AddWithValue("@6", product.year);
                tableCmd.Parameters.AddWithValue("@7", product.color);
                tableCmd.Parameters.AddWithValue("@8", @User.Identity?.Name);

                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7175/");
    }
    //hämtar alla produkter från databasen till en lista.
    internal ItemViewModel GetAllItems(string _filter ,string _order)
    {
        List<ItemModel> itemList = new();

        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                
                if (!String.IsNullOrEmpty(_filter))
                {
                    tableCmd.Parameters.AddWithValue("@0", _filter);

                    tableCmd.CommandText = "SELECT * FROM products4 WHERE description LIKE '%" + _filter + "%' OR title LIKE '%" + _filter + "%' ORDER BY "+_order;

                }
                else
                {
                    tableCmd.CommandText = "SELECT * FROM products4 ORDER BY "+_order;
                }
                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            itemList.Add(
                                new ItemModel
                                {
                                    category = reader.GetString(0),
                                    title = reader.GetString(1),
                                    price = reader.GetInt32(2),
                                    description = reader.GetString(3),
                                    image = reader.GetString(9),
                                    ProductID = reader.GetInt32(11)

                                });
                        }
                    }
                    else
                    {
                        return new ItemViewModel
                        {
                            ItemList = itemList
                        };
                    }
                };
            }
        }

        return new ItemViewModel
        {
            ItemList = itemList
        };
    }

    internal ItemViewModel GetUserItems()
    {
        List<ItemModel> userItemList = new();
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {

                con.Open();
                tableCmd.CommandText = "SELECT * FROM products4 WHERE username ='" + @User.Identity?.Name + "'";
                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userItemList.Add(
                                new ItemModel
                                {
                                    category = reader.GetString(0),
                                    title = reader.GetString(1),
                                    price = reader.GetInt32(2),
                                    description = reader.GetString(3),
                                    image = reader.GetString(9),
                                    ProductID = reader.GetInt32(11)

                                });
                        }
                    }
                    else
                    {
                        return new ItemViewModel
                        {
                            userItemList = userItemList
                        };
                    }
                };
                return new ItemViewModel
                {
                    userItemList = userItemList
                };
            }

        }



    }
    //Gilla-funktion
    public RedirectResult itemsLikedByUser(int id)
    {

        // Kolla om produkten redan är gillad.
        using (SqliteConnection con =
          new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                string txtSQL = "SELECT * FROM LinkTable WHERE ProductID ='" + id + "'AND CART = '0' AND username = '" + @User.Identity?.Name + "'";

                con.Open();

                tableCmd.CommandText = txtSQL;
                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        return Redirect("https://localhost:7175//Home/Index");
                    }
                    else
                    {

                    }

                };

            }

        }
        //Lägger till som gillad i LinkTable
        using (SqliteConnection con =
          new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                string txtSQL = "INSERT INTO LinkTable(Username,ProductID,Cart) VALUES (@0,@1,@2)";


                con.Open();

                tableCmd.CommandText = txtSQL;

                tableCmd.Parameters.AddWithValue("@0", @User.Identity?.Name);
                tableCmd.Parameters.AddWithValue("@1", id);
                tableCmd.Parameters.AddWithValue("@2", 0);


                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            return Redirect("https://localhost:7175/Home/Index");
        }

    }
    //hämtar users gillade produkter.
    internal ItemViewModel GetUserLikedItems()
    {
        //AND username = '" + userLoggedIn + "'
        List<ItemModel> userLikedItems = new();
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "SELECT products4.category,products4.title,products4.price,products4.description,products4.image,products4.ProductID FROM LinkTable INNER JOIN products4 ON LinkTable.ProductID = products4.ProductID WHERE LinkTable.Cart = 0 AND LinkTable.username ='" + @User.Identity?.Name + "'";
                try
                {
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                userLikedItems.Add(
                                    new ItemModel
                                    {
                                        category = reader.GetString(0),
                                        title = reader.GetString(1),
                                        price = reader.GetInt32(2),
                                        description = reader.GetString(3),
                                        image = reader.GetString(4),
                                        ProductID = reader.GetInt32(5)

                                    });
                            }
                        }
                        else
                        {
                            return new ItemViewModel
                            {
                                userLikedItems = userLikedItems
                            };
                        }
                    };
                    return new ItemViewModel
                    {
                        userLikedItems = userLikedItems
                    };

                }
                catch
                {
                    return new ItemViewModel
                    {
                        userLikedItems = userLikedItems
                    };
                }

            }
        }

    }

    //ogilla funktion
    public RedirectResult itemsUnLikedByUser(int id)
    {
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "DELETE FROM LinkTable WHERE productID= " + id + " AND CART = '0' AND LinkTable.username ='" + @User.Identity?.Name + "'";

                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7175/Home/LikedByUser");
    }

    public RedirectResult addToCart(int id)
    {

        // Kolla om produkten redan är tillagd i kundkorgen
        using (SqliteConnection con =
          new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                string txtSQL = "SELECT * FROM LinkTable WHERE ProductID ='" + id + "' AND CART = '1' AND username ='" + @User.Identity?.Name + "'";


                con.Open();

                tableCmd.CommandText = txtSQL;
                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        return Redirect("https://localhost:7175/Home/Index");
                    }
                    else
                    {

                    }

                };

            }
        }
        using (SqliteConnection con =
          new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                string txtSQL = "INSERT INTO LinkTable (username,ProductID,Cart) VALUES (@0,@1,@2)";


                con.Open();

                tableCmd.CommandText = txtSQL;

                tableCmd.Parameters.AddWithValue("@0", @User.Identity?.Name);
                tableCmd.Parameters.AddWithValue("@1", id);
                tableCmd.Parameters.AddWithValue("@2", 1);


                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            return Redirect("https://localhost:7175/Home/Index");
        }

    }
    internal ItemViewModel GetUserCartItems()
    {
        List<ItemModel> addedToCartList = new();
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "SELECT products4.category,products4.title,products4.price,products4.description,products4.image,products4.ProductID FROM LinkTable INNER JOIN products4 ON LinkTable.ProductID = products4.ProductID WHERE LinkTable.Cart = 1 AND LinkTable.username = '" + @User.Identity?.Name + "'";
                try
                {
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                addedToCartList.Add(
                                    new ItemModel
                                    {
                                        category = reader.GetString(0),
                                        title = reader.GetString(1),
                                        price = reader.GetInt32(2),
                                        description = reader.GetString(3),
                                        image = reader.GetString(4),
                                        ProductID = reader.GetInt32(5)

                                    });
                            }
                        }
                    };

                }
                catch
                {
                    return new ItemViewModel
                    {
                        addedToCartList = addedToCartList
                    };
                }

            }
        }
        int priceSum = 0;
        List<ItemModel> itemsInCart = new();
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "SELECT products4.category,products4.title,products4.price,products4.description,products4.image,products4.ProductID FROM LinkTable INNER JOIN products4 ON LinkTable.ProductID = products4.ProductID WHERE LinkTable.Cart = 1 AND LinkTable.username ='" + @User.Identity?.Name + "'";
                try
                {
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                itemsInCart.Add(
                                    new ItemModel
                                    {
                                        price = reader.GetInt32(2)
                                    });
                            }
                        }
                        else
                        {
                            foreach (ItemModel x in itemsInCart)
                            {
                                priceSum += x.price;

                            }
                            return new ItemViewModel
                            {
                                cartSum = priceSum,
                                addedToCartList = addedToCartList
                            };
                        }
                    };

                    foreach (ItemModel x in itemsInCart)
                    {
                        priceSum += x.price;
                    }
                    double priceWithMoms = (priceSum * 1.25) + 99;
                    double moms = priceSum * 0.25;
                    return new ItemViewModel
                    {
                        cartSum = priceSum,
                        finalPrice = priceWithMoms,
                        moms = moms,
                        addedToCartList = addedToCartList
                    };


                }
                catch
                {
                    return new ItemViewModel
                    {
                        cartSum = priceSum,
                        addedToCartList = addedToCartList
                    };
                }

            }
        }

    


}

    public RedirectResult itemsDeletedFromCart(int id)
    {
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "DELETE FROM LinkTable WHERE productID= " + id + " AND CART = '1' AND username ='" + @User.Identity?.Name + "'";

                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7175/Home/UserCart");
    }

    public RedirectResult deleteProduct(int id)
    {
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "DELETE FROM LinkTable WHERE productID= " + id;

                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "DELETE FROM products4 WHERE productID= " + id;

                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7175/Home/");
    }

    public RedirectResult continueCheckOut()
    {
        return Redirect("https://localhost:7175/Home/OrderPage");
    }

    public RedirectResult OrderMade(CheckOutModel order)
    {

        using (SqliteConnection con =
        new SqliteConnection("Data Source=db.sqlite"))
        {
            using (var tableCmd = con.CreateCommand())
            {
                string txtSQL = "INSERT INTO OrdersMade (FirstName,LastName,Address,ZIPCode,username) VALUES (@0,@1,@2,@3,@4)";


                con.Open();

                tableCmd.CommandText = txtSQL;

                tableCmd.Parameters.AddWithValue("@0", order.FirstName);
                tableCmd.Parameters.AddWithValue("@1", order.LastName);
                tableCmd.Parameters.AddWithValue("@2", order.Address);
                tableCmd.Parameters.AddWithValue("@3", order.ZipCode);
                tableCmd.Parameters.AddWithValue("@4", @User.Identity?.Name);

                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
        return Redirect("https://localhost:7175/");
    }

    
}

