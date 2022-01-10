using System;
using System.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Drawing.Imaging;
using System.Text;
using System.CodeDom;
using System.Linq;

namespace FormsTest
{

    public class clsReadMetaData
    {

        public DateTime GetDate(string filepath)
        {

            int year, month, day, hour, min, sec;
            year = month = day = hour = min = sec = 1;

            // Open file and read properties
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Image image1 = Image.FromStream(fs);
            PropertyItem[] imagePropertyItems = image1.PropertyItems;

            // Look for date created tag
            foreach (PropertyItem pi in imagePropertyItems)
            {
                if (pi.Id == 306)
                {
                    // Fetch tag byte array and convert to DateTime
                    string str = Encoding.UTF8.GetString(pi.Value, 0, pi.Value.Length);

                    year = Int32.Parse(str.Substring(0, 4));
                    month = Int32.Parse(str.Substring(5, 2));
                    day = Int32.Parse(str.Substring(8, 2));
                    hour = Int32.Parse(str.Substring(11, 2));
                    min = Int32.Parse(str.Substring(14, 2));
                    sec = Int32.Parse(str.Substring(17, 2));
                    break;
                }

            }

            DateTime dt = new DateTime(year, month, day, hour, min, sec);

            fs.Close();

            return (dt);
        }


        public void SetComment(string filein, string comment, string fileout)
        {
            // Open file and read properties
            FileStream fs = new FileStream(filein, FileMode.Open, FileAccess.Read);
            Image image1 = Image.FromStream(fs);
            PropertyItem[] imagePropertyItems = image1.PropertyItems;

            // Look for User Comment tag
            foreach (PropertyItem pi in imagePropertyItems)
            {
                if (pi.Id == 37510)
                {
                    // Update the tag value with input comment
                    //pi.Value = Encoding.UTF8.GetBytes(comment);
                    pi.Value = Encoding.ASCII.GetBytes(comment);
                    pi.Len = comment.Length;
                    image1.SetPropertyItem(pi);

                    // Save to output file
                    image1.Save(fileout);
                    fs.Close();
                    break;
                }
            }
        }

        public void SetComment(string filein, string comment)
        {

            // Output to a temp file and open it
            SetComment(filein, comment, "temp");
            FileStream fs = new FileStream("temp", FileMode.Open, FileAccess.Read);
 
            // Create a new User Comment property with the correct byte structure
            PropertyItem propertyItem = CreatePropertyItem();
            propertyItem.Id = Convert.ToInt32(37510);
            propertyItem.Type = 2;
            propertyItem.Len = comment.Length;
            propertyItem.Value = Encoding.ASCII.GetBytes(comment);
            Image image1 = Image.FromStream(fs);
            image1.SetPropertyItem(propertyItem);

            // Save it back to the current file and delete the temp file
            image1.Save(filein);
            fs.Close();
            File.Delete("temp");
        }
            private PropertyItem CreatePropertyItem()
        {
            System.Reflection.ConstructorInfo ci = typeof(PropertyItem).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            return (PropertyItem)ci.Invoke(null);
        }


        public string GetComment(string filepath)
        {
            string str = "";

            // Open file and read properties
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Image image1 = Image.FromStream(fs);
            PropertyItem[] imagePropertyItems = image1.PropertyItems;

            // Look for User Comment tag
            foreach (PropertyItem pi in imagePropertyItems)
            {
                if (pi.Id == 37510)
                {
                   str = Encoding.UTF8.GetString(pi.Value, 0, pi.Value.Length);
                   break;
                }

            }

            // Remove embedded '\0's
            string comment = "";
            if ((str.Length > 2) && (str[1] == '\0'))
            {
                //string comment = "";
                string str2 = "";
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] != '\0')
                    {
                        str2 += str[i];
                    }

                }

                // Remove junk at end
                int comend = str2.Length - 5;
                comment = str2.Substring(0, comend);
            }
            else
            {
                comment = str;
            }

            fs.Close();

            return (comment);
        }

    }

    public enum EXIFProperty
    {
        Title = 40091,
        Author = 40093,
        Keywords = 40094,
        Comments = 40092,
        Description = 270
    }
    public class ImageMetadata
    {
        private string _title = string.Empty;
        private string _author = string.Empty;
        private string _keywords = string.Empty;
        private string _comments = string.Empty;
        private string _description = string.Empty;
        public ImageMetadata()
        {
            this._title = string.Empty;
            this._author = string.Empty;
            this._keywords = string.Empty;
            this._comments = string.Empty;
            this._description = string.Empty;
        }
        public ImageMetadata(string title, string author, string keywords, string comments, string description)
        {
            this._title = title;
            this._author = author;
            this._keywords = keywords;
            this._comments = comments;
            this._description = description;
        }
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
        public string Author
        {
            get
            {
                return this._author;
            }
            set
            {
                this._author = value;
            }
        }
        public string Keywords
        {
            get
            {
                return this._keywords;
            }
            set
            {
                this._keywords = value;
            }
        }
        public string Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
            }
        }
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

    }  // End ImageMetData

}
