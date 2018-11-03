using BookLibrary.API.Entities;
using BookLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.API
{
    public static class ApplicationContextExtensions
    {
        private static List<Author> _authors;

        public static void EnsureSeedDataForContext(this ApplicationContext context)
        {
            if (context.Authors.Any())
            {
                return;
            }

            ReadFileAndPopulateDatabase("./Seeds/books.xml");

            // Update context and save all changes.
            context.Authors.AddRange(_authors);
            context.SaveChanges();
        }

        private static void ReadFileAndPopulateDatabase(String file)
        {
            /*
             * Read XML file data with helper into object.
             */
            String xmlfile = file;
            DataFromFile myFile = new Helpers.FileHelper().XML_File_To_Object<DataFromFile>(xmlfile);

            // Prepare list of authors.
            _authors = new List<Author>();

            // Parse through myFile.books to get the real book data.
            foreach (DataFromFile book in myFile.catalog["book"])
            {
                // Current book has no author yet = add it.
                if (_authors.Count == 0)
                {
                    /* 
                     * Use function defined above, authors passed as "ref" because its value changes and -
                     * we need the changes after the function was already executed (AddAuthorFromBook) 
                     */
                    AddAuthorFromBook(ref _authors, book);
                }
                else
                {
                    bool found = false;
                    int i = 0;

                    // Try to locate the author of the current book.
                    while (i < _authors.Count && found == false)
                    {
                        if (_authors.ElementAt(i).AuthorName == book.Author)
                        {
                            found = true;
                            continue;
                        }
                        i++;
                    }

                    // If the author is found.
                    if (found)
                    {
                        // Add this book to the same author, which has other book(s).
                        _authors.ElementAt(i).Books.Add(new Book()
                        {
                            Title = book.Title,
                            Genre = book.Genre,
                            Description = book.Description,
                            Price = book.Price,
                            PublishingDate = book.Publish_Date
                        });
                    }
                    else
                    {
                        // If author is not found, we need to add it.
                        AddAuthorFromBook(ref _authors, book);
                    }
                }
            }
        }

        private static void AddAuthorFromBook(ref List<Author> authors, DataFromFile book)
        {
            /*
             * Adding new author from Book - Book object contains author data.
             * Here book object is the version read from XML, with original structure,
             * but it is saved in the format used by the API - classes Author and Book. */
            authors.Add(new Author()
            {
                // Fetch author name.
                AuthorName = book.Author,
                Books = new List<Book>()
                    {
                        new Book()
                        {
                            Title = book.Title,
                            Genre = book.Genre,
                            Description = book.Description,
                            Price = book.Price,
                            PublishingDate = book.Publish_Date
                        }
                    }
            });
        }

    }
}
