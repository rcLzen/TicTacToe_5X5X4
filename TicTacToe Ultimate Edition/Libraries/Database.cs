using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TicTacToe_Ultimate_Edition.Libraries
{
    public class Database
    {
        private string path = "data.xml";
        private XmlDocument xDoc;

        public Database()//i made a change to this
        {

            //only creates xml database if the file does not exist
            if (!File.Exists(path))
            {
                XmlTextWriter xtw = new XmlTextWriter(path, Encoding.UTF8);
                xtw.Formatting = Formatting.Indented;

                //creates a empty database with the Table Users
                xtw.WriteStartDocument();
                xtw.WriteStartElement("Users");
                xtw.WriteEndElement();
                xtw.Close();
            }
        }

        
        public Player newUser(String name)
        {
            Player player = null;

            //only adds a user if he doesn't already exist
            if (!exists(name))
            {
                xDoc = new XmlDocument();
                FileStream fs = new FileStream(path, FileMode.Open);
                xDoc.Load(fs);

                XmlElement user = xDoc.CreateElement("User");

                //sets default stats to each  new user
                user.SetAttribute("Name", name);
                user.SetAttribute("Avatar", "pack://application:,,,/TicTacToe Ultimate Edition;component/Resources/Images/Avatars/Avatar01.png");
                user.SetAttribute("Win", "0");
                user.SetAttribute("Loss", "0");
                user.SetAttribute("Score", "0");
                xDoc.DocumentElement.AppendChild(user);

                fs.Close();
                xDoc.Save(path);

                player = new Player("pack://application:,,,/TicTacToe Ultimate Edition;component/Resources/Images/Avatars/Avatar01.png", name, 0);
            }

            return player;
        }

        //type is Win or Loss
        //depending on which value should be incremented
        public bool updateWinLoss(string userName, String type, int incScore = 0)
        {
            //shows if updating wins or losses is successful
            bool success = false;
            xDoc = new XmlDocument();
            FileStream fs = new FileStream(path, FileMode.Open);
            xDoc.Load(fs);

            XmlNodeList list = xDoc.GetElementsByTagName("User");

            //goes through each element until the element with the given name
            //is found
            for (int x = 0; x < list.Count; x++)
            {
                XmlElement name = (XmlElement)xDoc.GetElementsByTagName("User")[x];

                //if the name is found incrmenent the type by 1
                if (name.GetAttribute("Name") == userName)
                {
                    XmlElement win = (XmlElement)xDoc.GetElementsByTagName("User")[x];
                    XmlElement score = (XmlElement)xDoc.GetElementsByTagName("User")[x];

                    int value = Convert.ToInt16(win.GetAttribute(type)) + 1;
                    long add = Convert.ToInt64(score.GetAttribute("Score")) + incScore;
                    win.SetAttribute(type, Convert.ToString(value));
                    score.SetAttribute("Score", Convert.ToString(add));

                    success = true;
                }
            }

            fs.Close();
            xDoc.Save(path);

            return success;
        }

        public bool exists(String userName)
        {
            //shows if a name exists within the database
            bool doesExist = false;
            xDoc = new XmlDocument();
            FileStream fs = new FileStream(path, FileMode.Open);
            xDoc.Load(fs);

            XmlNodeList list = xDoc.GetElementsByTagName("User");

            //go through the list to see if the user already exists
            for (int x = 0; x < list.Count; x++)
            {
                XmlElement name = (XmlElement)xDoc.GetElementsByTagName("User")[x];
                if (name.GetAttribute("Name") == userName)
                    doesExist = true;
            }

            fs.Close();
            xDoc.Save(path);

            return doesExist;
        }

        //returns the names of all users in the database
        public String[] getUsers()
        {
            xDoc = new XmlDocument();
            FileStream fs = new FileStream(path, FileMode.Open);
            xDoc.Load(fs);

            XmlNodeList list = xDoc.GetElementsByTagName("User");
            String[] names = new String[list.Count];

            for (int x = 0; x < list.Count; x++)
            {
                XmlElement name = (XmlElement)xDoc.GetElementsByTagName("User")[x];
                names[x] = name.GetAttribute("Name");
            }

            fs.Close();
            xDoc.Save(path);
            Array.Sort(names);

            return names;
        }

        public Player[] getUserRanking()
        {
            xDoc = new XmlDocument();
            FileStream fs = new FileStream(path, FileMode.Open);
            xDoc.Load(fs);

            XmlNodeList list = xDoc.GetElementsByTagName("User");
            List<Player> names = new List<Player>();
            

            for (int x = 0; x < list.Count; x++)
            {
                XmlElement player = (XmlElement)xDoc.GetElementsByTagName("User")[x];
                string avatar = player.GetAttribute("Avatar");
                string name = player.GetAttribute("Name");
                long score = Convert.ToInt64(player.GetAttribute("Score"));
                int win = Convert.ToInt16(player.GetAttribute("Win"));
                int loss = Convert.ToInt16(player.GetAttribute("Loss"));

                names.Add(new Player(avatar, name, score, win, loss));
                Debug.WriteLine(x);
            }

            fs.Close();
            xDoc.Save(path);

            names = names.OrderByDescending(Player => Player.Score).ToList<Player>();

            return names.ToArray();
        }

        //for testing only
        public void refreshDb()
        {
            XmlTextWriter xtw = new XmlTextWriter(path, Encoding.UTF8);
            xtw.Formatting = Formatting.Indented;

            xtw.WriteStartDocument();
            xtw.WriteStartElement("Users");
            xtw.WriteEndElement();
            xtw.Close();
        }

        public Player getUser(string userID)
        {
            xDoc = new XmlDocument();
            FileStream fs = new FileStream(path, FileMode.Open);
            xDoc.Load(fs);

            XmlNodeList list = xDoc.GetElementsByTagName("User");
            Player player; ;

            fs.Close();
            xDoc.Save(path);

            for (int x = 0; x < list.Count; x++)
            {
                XmlElement user = (XmlElement)xDoc.GetElementsByTagName("User")[x];

                if (user.GetAttribute("Name") == userID)
                {

                    string avatar = user.GetAttribute("Avatar");
                    string name = user.GetAttribute("Name");
                    long score = Convert.ToInt64(user.GetAttribute("Score"));
                    int win = Convert.ToInt16(user.GetAttribute("Win"));
                    int loss = Convert.ToInt16(user.GetAttribute("Loss"));

                    return player = new Player(avatar, name, score, win, loss);
                }
            }

            return new Player("pack://application:,,,/TicTacToe Ultimate Edition;component/Resources/Images/Avatars/Avatar05.png", "Guest", 0);
        }

        public void updateAvatar(Player updateMe)
        {
            xDoc = new XmlDocument();
            FileStream fs = new FileStream(path, FileMode.Open);
            xDoc.Load(fs);

            XmlNodeList list = xDoc.GetElementsByTagName("User");

            for (int x = 0; x < list.Count; x++)
            {
                XmlElement name = (XmlElement)xDoc.GetElementsByTagName("User")[x];

                if (name.GetAttribute("Name") == updateMe.Name)
                {
                    XmlElement avatar = (XmlElement)xDoc.GetElementsByTagName("User")[x];

                    name.SetAttribute("Avatar", updateMe.Avatar);

                    Debug.WriteLine(name.GetAttribute("Avatar"));
                }
            }

            fs.Close();
            xDoc.Save(path);
        }
    }
}
