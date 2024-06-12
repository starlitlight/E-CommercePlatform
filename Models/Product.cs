using System;

public class Product : IDisposable
{
    public int Id { get; private set; } // Consider if set is really necessary publically
    public string Name { get; private set; } // Immutable after construction
    public string Category { get; private set; } // Immutable after construction
    public decimal Price { get; set; } // Price might change, so setter remains public
    private string _description; // Backing field for Description property

    public string Description 
    { 
        get => _description; 
        private set => _description = value; // Immutable after construction
    }

    // Constructor to enforce immutability after creation
    public Product(int id, string name, string category, decimal price, string description)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        Description = description;
    }

    // Implement IDisposable only if you eventually use unmanaged resources
    public void Dispose()
    {
        // Cleanup resources
    }
}