using APIProject.Models;

// Create the WebApplication builder.
var builder = WebApplication.CreateBuilder(args);



// ========== Services Configuration ==========

// Register MVC Controllers in the Dependency Injection container.
builder.Services.AddControllers();

// Register services required for generating API documentation.
builder.Services.AddEndpointsApiExplorer();

// Add Swagger generator service.
builder.Services.AddSwaggerGen();


// Build the WebApplication object.
var app = builder.Build();



//========== Middleware Configuration ==========

// Check if the application is running in Development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Enable Swagger JSON endpoint.
    app.UseSwaggerUI(); // Enable Swagger UI page for testing API endpoints.
}


// Redirect HTTP requests to HTTPS for security.
app.UseHttpsRedirection();

// Enables authorization middleware.
app.UseAuthorization();


// Maps Controller routes to the application.
app.MapControllers();

//========== Minimal API ==========



// Hardcoded data source
List<Book> books = new()
{

     new Book
            {
                Id = 1,
                Title = "Harry Potter",
                Description ="Amazing book",
                Price = 25
            },

            new Book
            {
                Id = 2,
                Title = "Clean Code",
               Description="Greate book ",
                Price = 40
            },

            new Book
            {
                Id = 3,
                Title = "The Hobbit",
                Description="Good book",
                Price = 30
            }


};



//========== Minimal API GET ALL ===========

// GET: /minimal/books
// Api to Return Books
app.MapGet("/minimal/books", () =>
{

    return books;

});




// ========== Minimal API GET BY ID ==========

// GET: /minimal/books/1
//Api to return a book with a specific id

app.MapGet("/minimal/books/{id}", (int id) =>
{
    //Searching for the book
    var book = books.FirstOrDefault(b => b.Id == id);


    if (book == null)
    {
        return Results.NotFound();
    }


    return Results.Ok(book);

});

app.Run();