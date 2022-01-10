using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Drawing.Imaging;
using System.Text;


namespace PhotoDoc2
{
    public class TagIO
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
            bool found = false;
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
                    found = true;
                    break;
                }
            }

            // Check if tag not found - create it
            if (!found)
            {
                PropertyItem propertyItem = CreatePropertyItem();
                propertyItem.Id = Convert.ToInt32(37510);
                propertyItem.Type = 2;
                propertyItem.Len = comment.Length;
                //propertyItem.Value = Encoding.Unicode.GetBytes(propertyValue)
                propertyItem.Value = Encoding.UTF8.GetBytes(comment);
                image1.SetPropertyItem(propertyItem);
                image1.Save(fileout);
            }

            fs.Close();
 
        }

        public void SetComment(string filein, string comment)
        {
            // Output to a temp file and open it
            SetComment(filein, comment, "temp");
            File.Copy("temp", filein, true);
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

        public bool IsPortrait(string filepath)
        {
            bool rtn = false;

            // Open file and read properties
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Image image1 = Image.FromStream(fs);
            PropertyItem[] imagePropertyItems = image1.PropertyItems;

            // Look for Orientation tag
            foreach (PropertyItem pi in imagePropertyItems)
            {
                if (pi.Id == 274)
                {                  
                    string str = Encoding.UTF8.GetString(pi.Value, 0, pi.Value.Length);
                    if (str == "\u0006\0")
                    {
                        rtn = true;
                    }
                    break;

                }
            }

            return (rtn);
        }


        }
}