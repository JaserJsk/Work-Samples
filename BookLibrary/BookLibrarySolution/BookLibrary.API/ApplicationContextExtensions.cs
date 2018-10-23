using BookLibrary.API.Entities;
using BookLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.API
{
    public static class ApplicationContextExtensions
    {
        private static void AddAuthorFromBook(ref List<Author> authors, DataFromFile book)
        {
            /*
             * Adding new author from Book - Book object contains author data
             * Here book object is the version read from XML, with original structure,
             * but it is saved in the format used by the API - classes Author and Book. */
            authors.Add(new Author()
            {
                // Fetch author name
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

        public static void EnsureSeedDataForContext(this ApplicationContext context)
        {
            if (context.Authors.Any())
            {
                return;
            }

            // Prepare list of authors
            var authors = new List<Author>();

            /*
             * Read XML data with helper into myFile object
             */
            String xmlfile = "./Seeds/xml/books.xml";
            DataFromFile myFile = new Helpers.FileHelper().XML_File_To_Object<DataFromFile>(xmlfile);

            // Parse through myFile.books to get the real book data
            foreach (DataFromFile book in myFile.catalog["book"])
            {
                // Current book has no author yet = add it
                if (authors.Count == 0)
                {
                    // Use function defined above, authors passed as "ref" because its value changes and
                    // We need the changes after the function was already executed (AddAuthorFromBook)
                    AddAuthorFromBook(ref authors, book);
                }
                else
                {
                    bool found = false;
                    int i = 0;

                    // Try to locate the author of the current book
                    while (i < authors.Count && found == false)
                    {
                        if (authors.ElementAt(i).AuthorName == book.Author)
                        {
                            found = true;
                            continue;
                        }
                        i++;
                    }

                    // If the author is found
                    if (found)
                    {
                        // Add this book to the same author, which has other book(s)
                        authors.ElementAt(i).Books.Add(new Book()
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
                        // If author is not found, we need to add it
                        AddAuthorFromBook(ref authors, book);
                    }
                }
            }

            // Update context to save all changes with authors
            context.Authors.AddRange(authors);
            context.SaveChanges();

        }

    }
}
