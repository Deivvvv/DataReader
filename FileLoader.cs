using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using UnityEngine.Tilemaps;
using World;

namespace FileLoader
{
    static class Loader
    {
        static string mainPath = Application.dataPath + "/Resources/";
        

        public static void LoadQuest(){//List<Quest>
       // List<Quest> quest = new List<Quest>();
        }
        public static List<NoiseMap> LoadNoise()
        {
            List<NoiseMap> words = new List<NoiseMap>();
            XmlDocument root = new XmlDocument();
            root.Load(mainPath + "Noise.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                string[] com = node.Element("Tayp").Value.Split('_');
                NoiseMap noise = new NoiseMap(node.Element("Name").Value, int.Parse(com[0]), int.Parse(com[1]), int.Parse(com[2]));

                words.Add(noise);
            }
            return words;
        }
        public static List<GeneratorTayp> LoadTileSet(string str)
        {
            List <GeneratorTayp> words = new List<GeneratorTayp>();
            XmlDocument root = new XmlDocument();
            root.Load(mainPath + str+".xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here
            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                GeneratorTayp generator = new GeneratorTayp(node.Element("Name").Value, node.Element("Tayp").Value);

                words.Add(generator);
            }
            return words;
        }

        public static List<TileCase> LoadTiles(string[] com)
        {
           // !разбить загрузку на две ступени, сначала присваивам все тайлы, а потом загружаем логику
            List<TileCase> words = new List<TileCase>();
            foreach (string str in com) 
            {
                XmlDocument root = new XmlDocument();
                root.Load(mainPath + str + ".xml");
                XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
                XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here
                foreach (XmlNode x in nodes)
                {
                    node = XElement.Load(new XmlNodeReader(x));
                    TileCase tile = new TileCase(node.Element("Name").Value);
                   // Debug.Log(tile.Name);
                    tile.SetTayp(node.Element("Tayp").Value);

                    XmlNodeList nodesl = x.SelectNodes("descendant::NeedTile"); // You can also use XPath here
                    //Обснастить систему конверторами данных для условий тайлов, тчобы ссылатсья на них сразу, а не конверитровать каждый раз
                    foreach (XmlNode z in nodesl)
                    {
                        XElement nodeL = XElement.Load(new XmlNodeReader(z));
                        tile.SetNeedTile(nodeL.Value);
                    }
                    words.Add(tile);
                }
            }

            int h = 0;
            for(int i=0;i<com.Length;i++)
            {
                XmlDocument root = new XmlDocument();
                root.Load(mainPath + com[i] + ".xml");
                XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
                XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here
                for(int j=0;j< nodes.Count;j++)
                //foreach (XmlNode x in nodes)
                {
                    TileCase tile = words[h];
                    BuildCallCase bc = new BuildCallCase();
                    XmlNodeList nodesl = nodes[j].SelectNodes("descendant::Need"); // You can also use XPath here
                    for (int k = 0; k < nodesl.Count; k++)
                    {

                        XmlNodeList nodels = nodesl[k].SelectNodes("descendant::Resurses"); // You can also use XPath here
                        foreach (XmlNode x in nodels)
                        {
                            XElement nodeL = XElement.Load(new XmlNodeReader(x));
                            bc.AddCall(nodeL.Value);
                        }
                        tile.AddCall(bc);
                    }
                }
                h++;
            }


            //for (int i = 0; i < words.Count; i++)
            //    for (int j = 0; j < words[i].tileCases.Count; j++)
            //        words[i].tileCases[j].Id = new intM(i,j);


            return words;
        }
        public static List<CharterData> LoadCharters()
        {
            List<CharterData> words = new List<CharterData>();
            XmlDocument root = new XmlDocument();
            root.Load(mainPath + "Charter.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here

            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                CharterData data = new CharterData(node.Element("Name").Value);
                //data.Set(node.Element("Value").Value);

                words.Add(data);
            }
            words.OrderBy(x => x.Name).ToList();

            return words;
        }

        public static List<ParametrData> LoadParametr()
        {
            List<ParametrData> words = new List<ParametrData>();
            XmlDocument root = new XmlDocument();
            root.Load(mainPath + "Parametr.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here

            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                ParametrData data = new ParametrData(node.Element("Name").Value);
                data.Set(node.Element("Value").Value);

                words.Add(data);
            }
            words.OrderBy(x => x.Name).ToList();

            return words;
        }

        public static List<RoleList> LoadRoles()
        {
            List<RoleList> words = new List<RoleList>();
            XmlDocument root = new XmlDocument();
            root.Load(mainPath + "Roles.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here

            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                words.Add(new RoleList(node.Element("Name").Value));
            }
            words.OrderBy(x => x.Name).ToList();

            return words;
        }
        public static List<Approval> LoadApprovals()
        {
            List<Approval> list = new List<Approval>();
            XmlDocument root = new XmlDocument();
            root.Load(mainPath + "Approvals.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here

            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                list.Add(new Approval(node.Element("Name").Value, bool.Parse(node.Element("Bool").Value)));
            }
            list.OrderBy(x => x.Name).ToList();

            return list;
        }

        public static List<Counter> LoadCounters()
        {
            List<Counter> list = new List<Counter>();
            XmlDocument root = new XmlDocument();
            root.Load(mainPath + "Counters.xml");
            XElement node;// = XDocument.Parse(File.ReadAllText(mainPath +"Roles.xml")).Element("root");
            XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Body"); // You can also use XPath here

            foreach (XmlNode x in nodes)
            {
                node = XElement.Load(new XmlNodeReader(x));
                list.Add(new Counter(node.Element("Name").Value, node.Element("Tayp").Value));
            }
            list.OrderBy(x => x.Name).ToList();

            return list;

        }

        public static void SaveScene(SceneCase story)
        {
            if (story == null)
                return;
            if (story.OrigName != story.Name )
            {
                DeliteScene(story.OrigName);
                story.OrigName = story.Name;
                //for (int i = 0; i < story.Links.Count; i++)
                //    SaveScene(GameData.GetScene(story.Links[i].Id));
            }
            XElement root = new XElement("root");
            root.Add(new XElement("Name", story.Name));
            root.Add(new XElement("Tag", story.Tag));
            root.Add(new XElement("BackLink", story.BackLink));
            root.Add(new XElement("Link", GameData.SceneConverter( story.Link)));
            for(int i = 0; i < story.Actions.Count; i++)
            {
                ActionCase actions = story.Actions[i];
                XElement action = new XElement("Action");
                if (actions.Texts == "")
                    actions.Texts = " ";
                if (actions.MeTexts == "")
                    actions.MeTexts = " ";
                action.Add(new XElement("Text", actions.Texts)); 
                action.Add(new XElement("MeText", actions.MeTexts)); 
                action.Add(new XElement("Emotion", actions.Emotion));
                action.Add(new XElement("Role", actions.Role));

                action.Add(new XElement("Link", GameData.SceneConverter(actions.Link)));

                action.Add(new XElement("BG", GameData.ImageConverter("BG",actions.Bg)));
                for (int k = 0; k < actions.Logic.Count; k++)
                {
                    LogicAction logic = actions.Logic[k];
                    XElement logicBody = new XElement("LogicBody");
                    logicBody.Add(new XElement("Mood", logic.Mood));
                    for (int j = 0; j < logic.Nums.Count; j++)
                    {
                        logicBody.Add(new XElement("LogicCase", LogicConverter(logic.Nums[j])));
                        string LogicConverter(LogicActionCase logicCase)
                        {
                            List<int> num = logicCase.Num;
                            string str = logicCase.Mood;
                                switch (str)
                                {
                                    case ("tayp"):
                                        {
                                            List<string> strs = GameData.GetWords("tayp");
                                            string ls = strs[num[0]];
                                            str += "_" + ls;
                                            str += "_" + GameData.GetWord(ls, num[1]);

                                            str += "_" + GameData.GetWord("roleGroup", num[2]);
                                            str += "_" + GameData.GetWord("parametr", num[3]);
                                            int s = GameData.ParametrSize(num[3]);
                                            str += "_" + GameData.GetWord("parametr" + s, num[4]);

                                        }
                                        break;
                                    case ("gtayp"):
                                        {
                                            List<string> strs = GameData.GetWords("gtayp");
                                            string ls = strs[num[0]];
                                            str += "_" + ls;
                                            str += "_" + GameData.GetWord(ls, num[1]);
                                            if(ls == "role")
                                                str += "_" + GameData.GetWord("roleGroup", num[2]);
                                    }
                                        break;
                                    case ("role"):
                                    {
                                        str += "_" + GameData.GetWord("role", num[0]);
                                        str += "_" + GameData.GetWord("roleGroup", num[1]);
                                    }
                                    break;
                                case ("i"):
                                        str += "_" + num[0];
                                        break;
                                    default:
                                        str += "_" + GameData.GetWord(logicCase.Mood, num[0]);
                                        break;
                                }
                            return str;
                        }
                    }

                    action.Add(logicBody);
                }

                root.Add(action);
            }

            XDocument saveDoc = new XDocument(root);
            File.WriteAllText($"{mainPath}Scenes/{story.Name}.xml", saveDoc.ToString());
        }
        public static void DeliteScene(string str)
        {
            File.Delete($"{mainPath}Scenes/{str}.xml");
        }
        public static void LoadScenes(bool editor)
        {
            List<SceneCase> stories = new List<SceneCase>();

            string[] com = Directory.GetFiles($"{mainPath}Scenes/", "*.xml");
            for (int i = 0; i < com.Length; i++)
            {
                XmlDocument root = new XmlDocument();
                root.Load(com[i]);


                XElement node = XDocument.Parse(File.ReadAllText(com[i])).Element("root");

                SceneCase scene = new SceneCase(null);
                scene.Name = scene.OrigName = node.Element("Name").Value;
                scene.Tag = int.Parse(node.Element("Tag").Value);
                scene.HardLink = node.Element("Link").Value;
                scene.BackLink = bool.Parse(node.Element("BackLink").Value);

                XmlNodeList nodes = root.DocumentElement.SelectNodes("descendant::Action"); // You can also use XPath here

                Debug.Log(com[i]);
                foreach (XmlNode x in nodes)
                {
                    node = XElement.Load(new XmlNodeReader(x));
                    ActionCase actionCase = new ActionCase();
                    actionCase.Texts = node.Element("Text").Value;

                    actionCase.MeTexts = node.Element("MeText").Value;
                    actionCase.Emotion = int.Parse(node.Element("Emotion").Value);
                    actionCase.Role = int.Parse(node.Element("Role").Value);
                    actionCase.HardLink = node.Element("Link").Value;

                    actionCase.Bg = GameData.ImageConverter(node.Element("BG").Value);

                    XmlNodeList nodesl = x.SelectNodes("descendant::LogicBody"); // You can also use XPath here

                    foreach (XmlNode z in nodesl)
                    {
                        node = XElement.Load(new XmlNodeReader(z));
                        LogicAction logic = new LogicAction(int.Parse(node.Element("Mood").Value));

                        XmlNodeList nodeslc = z.SelectNodes("descendant::LogicCase"); // You can also use XPath here
                        foreach (XmlNode c in nodeslc)
                        {
                            node = XElement.Load(new XmlNodeReader(c));
                            List<string> str = node.Value.Split('_').ToList();
                            List<int> nums = GameData.WordConverter(str);
                            LogicActionCase logicC = new LogicActionCase(str[0],nums.Count);
                            for (int l = 0; l < nums.Count; l++)
                                logicC.Num[l] = nums[l];

                            logic.Nums.Add(logicC);
                        }
                        actionCase.Logic.Add(logic);
                    }

                        scene.Actions.Add(actionCase);
                }

                stories.Add(scene);
            }
            if(editor)
                stories.OrderBy(x => x.Tag).ThenBy(x => x.Name).ToList();

            GameData.SetScenes(stories);



            for(int i=0;i<stories.Count;i++)
            {
                SceneCase scene = stories[i];
                scene.Link = GameData.SceneConverter(scene.HardLink);
                scene.HardLink = "";
                if (editor && scene.Link >= 0)
                    GameData.GetScene(scene.Link).AddLink(i, -1);
                

                for (int j = 0; j < scene.Actions.Count; j++)
                {
                    ActionCase action =scene.Actions[j];
                    action.Link = GameData.SceneConverter(action.HardLink);

                    if (editor && action.Link >=0)
                        GameData.GetScene(action.Link).AddLink(i,j);
                    
                    action.HardLink = "";
                }

            }
        }


     
    }
}

