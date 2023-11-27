﻿using MongoDB.Bson;
using SmartLibraryManager.Models;
using SmartLibraryManager.Utilties;

namespace SmartLibraryManager.Common.IntitialData
{
    public class BookStoreData
    {
        public List<Author> GetAuthors()
        {
            return new List<Author>()
            {
                new Author() { Id = ObjectId.GenerateNewId().ToString(), AuthorId = Convert.ToInt32(Util.GetRandomNumber()) , Name = "J.K. Rowling"},
                new Author() { Id = ObjectId.GenerateNewId().ToString(), AuthorId = Convert.ToInt32(Util.GetRandomNumber()) , Name = "Chetan Bagath"},
                new Author() { Id = ObjectId.GenerateNewId().ToString(), AuthorId = Convert.ToInt32(Util.GetRandomNumber()) , Name = "Dan Brown" },
                new Author() { Id = ObjectId.GenerateNewId().ToString(), AuthorId = Convert.ToInt32(Util.GetRandomNumber()) , Name = "J.R.R. Tolkien" },
                new Author() { Id = ObjectId.GenerateNewId().ToString(), AuthorId = Convert.ToInt32(Util.GetRandomNumber()) , Name = "Stephen King" },
            };
        }

        public List<BookCategory> GetBookCategories()
        {
            return new List<BookCategory>()
            {
                new BookCategory(){Id=  ObjectId.GenerateNewId().ToString(),CategoryId=Convert.ToInt32(Util.GetRandomNumber()),CategoryName="Adventure"},
                new BookCategory(){Id=  ObjectId.GenerateNewId().ToString(),CategoryId=Convert.ToInt32(Util.GetRandomNumber()),CategoryName="Science"},
                new BookCategory(){Id=  ObjectId.GenerateNewId().ToString(),CategoryId=Convert.ToInt32(Util.GetRandomNumber()),CategoryName="Maths"},
                new BookCategory(){Id=  ObjectId.GenerateNewId().ToString(),CategoryId=Convert.ToInt32(Util.GetRandomNumber()),CategoryName="Software"},
                new BookCategory(){Id=  ObjectId.GenerateNewId().ToString(),CategoryId=Convert.ToInt32(Util.GetRandomNumber()),CategoryName="Electrical"},
            };
        }


        public List<User> GetUsers()
        {
            return new List<User>()
            {
                new User(){FirstName="Shiva",LastName="Ch",Email="sxc20730@ucmo.edu",Password="123456",Phone="1234567890",Address="123, Main Street, New York",City="New York",UserId=Util.GetRandomNumber(),Id=ObjectId.GenerateNewId().ToString(),IsAdmin=true,Fine=0 },
                new User(){FirstName="Sai",LastName="Ch",Email="pintuchakilam666@gmail.com",Password="123456",Phone="1234567890",Address="123, Main Street, New York",City="New York",UserId=Util.GetRandomNumber(),Id=ObjectId.GenerateNewId().ToString(),IsAdmin=false,Fine=0 },
            };
        }

        public List<Book> GetBooks()
        {
            return new List<Book>()
            {
                 new Book(){
                Id = ObjectId.GenerateNewId().ToString(),
                BookId = Guid.NewGuid().ToString(),
                Title = "Unlocking Android",
                Author = "Charlie Collins",
                Category = "Technology",
                Description = "Unlocking Android: A Developer's Guide provides concise, hands-on instruction for the Android operating system and development tools. This book teaches important architectural concepts in a straightforward writing style and builds on this with practical and useful examples throughout.",
                PublishedYear = 1997,
                Status = "Available",
                ISBN = "9780747532743",
                isAvailable = true,
                Image = "https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                bookTransactions = new List<BookTransaction>(),

            },
                 new Book(){
                Id = ObjectId.GenerateNewId().ToString(),
                BookId = Guid.NewGuid().ToString(),
                Title = "Spider Man",
                Author = "Stan Lee",
                Category = "Comics",
                Description = "But this ordinary teenage boy is about to have his life turned upside down, when he is bitten by a genetically altered spider. Suddenly, he finds himself possessed of spectacular powers. He is now and forever Spider-Man!",
                PublishedYear = 1991,
                Status = "Available",
                isAvailable = true,
                ISBN = "9780747532743",
                Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                bookTransactions = new List<BookTransaction>(),

            },
                new Book(){
                Id = ObjectId.GenerateNewId().ToString(),
                BookId = Guid.NewGuid().ToString(),
                Title = "Learning SQL",
                Description = "Learn and skillsup",
                Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                ISBN = "test",
                Category = "Education",
                PublishedYear = 1998,
                Author = "Alan brealuie",
                Status = "Available",
                isAvailable = true,
                bookTransactions = new List<BookTransaction>()
                    },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                BookId = Guid.NewGuid().ToString(),
                Title = "Skill up",
                Description = "Learn and skillsup",
                Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                ISBN = "test",
                Category = "Education",
                PublishedYear = 1998,
                Author = "Robert treste",
                Status = "Available",
                isAvailable = true,
                bookTransactions = new List<BookTransaction>()
                    },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                BookId = Guid.NewGuid().ToString(),
                Title = "Target Goal I",
                Description = "Learn and skillsup",
                Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                ISBN = "test",
                Category = "Education",
                PublishedYear = 1998,
                Author = "ALex Martin",
                Status = "Available",
                isAvailable = true,
                bookTransactions = new List<BookTransaction>()
                    },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                BookId = Guid.NewGuid().ToString(),
                Title = "JAVA",
                Description = "Learn and skillsup",
                    Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                ISBN = "test",
                Category = "Education",
                PublishedYear = 1998,
                Author = "Alan brealuie",
                Status = "Available",
                isAvailable = true,
                bookTransactions = new List<BookTransaction>()
                    },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                BookId = Guid.NewGuid().ToString(),
                Title = "Target Goal II",
                Description = "Learn and skillsup",
                 Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                ISBN = "test",
                Category = "Education",
                PublishedYear = 1998,
                Author = "Alan brealuie",
                Status = "Available",
                isAvailable = true,
                bookTransactions = new List<BookTransaction>()
                    },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                BookId =Guid.NewGuid().ToString(),
                Title = "Target Goal",
                Description = "Learn and skillsup",
                 Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                ISBN = "test",
                Category = "Education",
                PublishedYear = 1998,
                Author = "Alan brealuie",
                Status = "Available",
                isAvailable = true,
                bookTransactions = new List<BookTransaction>()
                    },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                BookId =Guid.NewGuid().ToString(),
                Title = "Target Goal III",
                Description = "Learn and skillsup",
                Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                ISBN = "test",
                Category = "Education",
                PublishedYear = 1998,
                Author = "Alan brealuie",
                Status = "Available",
                isAvailable = true,
                bookTransactions = new List<BookTransaction>()
                    },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                            BookId =Guid.NewGuid().ToString(),
                            Title = "Harry Potter and the Philosopher's Stone",
                            Author = "J.K. Rowling",
                            Category = "Adventure",
                            Description = "Harry Potter and the Philosopher's Stone is a fantasy novel written by British author J. K. Rowling. The first novel in the Harry Potter series and Rowling's debut novel, it follows Harry Potter, a young wizard who discovers his magical heritage on his eleventh birthday, when he receives a letter of acceptance to Hogwarts School of Witchcraft and Wizardry. Harry makes close friends and a few enemies during his first year at the school, and with the help of his friends, Harry faces an attempted comeback by the dark wizard Lord Voldemort, who killed Harry's parents, but failed to kill Harry when he was just 15 months old.",
                            PublishedYear = 1997,
                            Status = "Available",
                            isAvailable = true,
                            ISBN = "9780747532743",
                            Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                            bookTransactions = new List<BookTransaction>()

                        },
                             new Book()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                            BookId =Guid.NewGuid().ToString(),
                            Title = "Harry Potter and the Chamber of Secrets",
                            Author = "J.K. Rowling",
                            Category = "Adventure",
                            Description = "Harry Potter and the Chamber of Secrets is a fantasy novel written by British author J. K. Rowling and the second novel in the Harry Potter series. The plot follows Harry's second year at Hogwarts School of Witchcraft and Wizardry, during which a series of messages on the walls of the school's corridors warn that the \"Chamber of Secrets\" has been opened and that the \"heir of Slytherin\" would kill all pupils who do not come from all-magical families. These threats are found after attacks which leave residents of the school petrified. Throughout the year, Harry and his friends Ron and Hermione investigate the attacks.",
                            PublishedYear = 1998,
                            Status = "Available",
                            isAvailable = true,
                            ISBN = "9780747538495",
                            Image="https://smartlibrarystorage.blob.core.windows.net/libraryassets/book-cartoon_22350-95.avif",
                            bookTransactions = new List<BookTransaction>(),

                        }
                        };

        }
    }

}
