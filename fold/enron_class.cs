using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proximity_placement
{
    class pp_class
    {
        public List<string> initEnronList;
        public List<string> dictionaryList;
        public List<string> source = new List<string>();
        public List<string> target = new List<string>();
        public List<int> indexSource = new List<int>();
        public List<int> indexTarget = new List<int>();
        public List<int> outgoing = new List<int>();
        public List<int> incoming = new List<int>();
        public List<int> indexGalapagosList;
        public List<int> connectionWeight;
        public List<int> clusterIndex = new List<int>();

        public List<string> mIN = new List<string>();
        public List<int> mII = new List<int>();
        public List<int> iC = new List<int>();

        public pp_class(List<string> enronST, List<int> indexGalapagos)
        {
            indexGalapagosList = indexGalapagos;
            
            initEnronList = cleanup(enronST);
            dictionaryList = dictionary(initEnronList);

            splitST(initEnronList, source, target);
            outConnection(outgoing, dictionaryList, source, target);
            inConnection(incoming, dictionaryList, source, target);
            indexSourceTarget(dictionaryList, source, target, indexSource, indexTarget);
            connectionWeight = calcConnectionWeight(indexSource, indexTarget);

            calcClusters(dictionaryList, source, target, incoming, outgoing, mIN, mII, iC);
        }
        


        //CLEAN UP
        private List<string> cleanup(List<string> s)
        {

            List<string> cleaned = new List<string>();

            //to lower case
            for (int i = 0; i < s.Count(); i++)
            {
                cleaned.Add(s[i].ToLower());
            }

            //remove . in front of name
            for (int i = 0; i < cleaned.Count(); i++)
            {
                if (cleaned[i].ElementAt(0) == '.')
                {
                    cleaned[i] = cleaned[i].Remove(0);
                }
            }

            //cull double points or single points at the end or beginning of string
            for (int i = 0; i < cleaned.Count(); i++)
            {
                for (int j = 0; j < cleaned[i].Count(); j++)
                {
                    if (j > 0)
                    {
                        //cull double points
                        if (cleaned[i][j] == '.' && cleaned[i][j - 1] == '.')
                        {
                            cleaned[i] = cleaned[i].Remove(j, 1);
                        }

                        //cull point at the end or beginning of string
                        if (cleaned[i][j] == ',' && cleaned[i][j - 1] == '.')
                        {
                            cleaned[i] = cleaned[i].Remove(j - 1, 1);
                        }
                        if (cleaned[i][j] == '.' && cleaned[i][j - 1] == ',')
                        {
                            cleaned[i] = cleaned[i].Remove(j, 1);
                        }
                    }
                    else
                    {
                        //cull double points
                        if (cleaned[i][j] == '.' && cleaned[i][j + 1] == '.')
                        {
                            cleaned[i] = cleaned[i].Remove(j, 1);
                        }

                        //cull point at the end or beginning of string
                        if (cleaned[i][j] == ',' && cleaned[i][j + 1] == '.')
                        {
                            cleaned[i] = cleaned[i].Remove(j + 1, 1);
                        }
                        if (cleaned[i][j] == '.' && cleaned[i][j + 1] == ',')
                        {
                            cleaned[i] = cleaned[i].Remove(j, 1);
                        }
                    }
                }
            }

            //cull source,target
            if (cleaned[0] == "source,target")
            {
                cleaned.RemoveAt(0);
            }

            //split by comma
            List<string> seperated = new List<string>();
            for (int i = 0; i < cleaned.Count(); i++)
            {
                string temp = "";
                for (int j = 0; j < cleaned[i].Count(); j++)
                {
                    if (cleaned[i].ElementAt(j) != ',')
                    {
                        temp += cleaned[i].ElementAt(j);
                    }
                    else
                    {
                        seperated.Add(temp);
                        temp = "";
                    }
                    if (j == cleaned[i].Count() - 1 && temp != "")
                    {
                        seperated.Add(temp);
                        temp = "";
                    }
                }
            }
            return seperated;
        }

        //SEPERATE INTO SOURCE AND TARGET
        private void splitST(List<string> st, List<string> s, List<string> t)
        {
            for (int i = 0; i < st.Count() - 1; i = i + 2)
            {
                s.Add(st[i]);
                t.Add(st[i + 1]);
            }
        }

        //SORT LIST OF NAMES
        private List<string> dictionary(List<string> s)
        {
            List<string> sorted = new List<string>();
            sorted = s;
            sorted.Sort();

            List<string> culled = new List<string>();

            for (int i = 0; i < sorted.Count(); i++)
            {
                if (i < sorted.Count() - 2)
                {
                    if (sorted.ElementAt(i) != sorted.ElementAt(i + 1))
                    {
                        //sorted.RemoveAt(i);
                        culled.Add(sorted.ElementAt(i));
                    }
                }
                else
                {
                    culled.Add(sorted.ElementAt(i));
                }
            }
            return culled;
        }

        //OUTGOING EMAILS
        private void outConnection(List<int> out_count, List<string> name, List<string> source, List<string> target)
        {
            for (int i = 0; i < name.Count(); i++)
            {
                out_count.Add(0);
            }

            for (int i = 0; i < source.Count(); i++)
            {
                for (int j = 0; j < name.Count(); j++)
                {
                    if (name[j] == source[i])
                    {
                        out_count[j]++;
                    }
                }
            }
        }

        //INCOMING EMAILS
        private void inConnection(List<int> in_count, List<string> name, List<string> source, List<string> target)
        {
            for (int i = 0; i < name.Count(); i++)
            {
                in_count.Add(0);
            }

            for (int i = 0; i < target.Count(); i++)
            {
                for (int j = 0; j < name.Count(); j++)
                {
                    if (name[j] == target[i])
                    {
                        in_count[j]++;
                    }
                }
            }
        }

        //SOURCE TARGET INDEX
        public void indexSourceTarget(List<string> dict, List<string> s, List<string> t, List<int> s_id, List<int> t_id)
        {
            List<int> id = new List<int>();
            for (int i = 0; i < dict.Count(); i++)
            {
                id.Add(i);
            }

            for (int i = 0; i < s.Count(); i++)
            {
                for (int j = 0; j < dict.Count(); j++)
                {
                    if (s[i] == dict[j])
                    {
                        s_id.Add(j);
                    }
                    if (t[i] == dict[j])
                    {
                        t_id.Add(j);
                    }
                }
            }
        }

        //CONNECTION WEIGHT
        public List<int> calcConnectionWeight(List<int> s_id, List<int> t_id)
        {
            List<int> cw = new List<int>();
            int count = s_id.Count();

            for (int i = 0; i < count; i++)
            {
                cw.Add(0);
            }

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (s_id[i] == t_id[j] && t_id[i] == s_id[j])
                    {
                        cw[i]++;
                    }
                    if (s_id[i] == s_id[j] && t_id[i] == t_id[j])
                    {
                        cw[i]++;
                    }
                }
            }

            return cw;
        }

        //CLUSTER
        public void calcClusters(List<string> name, List<string> source, List<string> target, List<int> incoming, List<int> outgoing, List<string> mostIN, List<int> mostII, List<int> influenceC) {

            List<int> relevance = new List<int>();
            for(int i = 0; i < incoming.Count(); i++)
            {
                relevance.Add(incoming[i] + outgoing[i]);
            }

            List<int> relevanceSorted = new List<int>();
            for (int i = 0; i < relevance.Count(); i++)
            {
                relevanceSorted.Add(relevance[i]);
            }

            relevanceSorted.Sort();
            relevanceSorted.Reverse();

            for (int i = 0; i < 10; i++)
            {
                mostIN.Add("null");
                mostII.Add(name.Count() + 1);
                influenceC.Add(relevanceSorted[i]);
            }

            for (int i = 0; i < influenceC.Count(); i++)
            {
                for(int j = 0; j < relevance.Count(); j++)
                {
                    if(influenceC[i] == relevance[j] && mostII[i] == name.Count() + 1)
                    {
                        mostII[i] = j;
                        mostIN[i] = name[j];
                    }
                }
            }
        }
    }
}


/*
public class Person
{
    // Field
    public string name;

    // Constructor that takes no arguments.
    public Person()
    {
        name = "unknown";
    }

    // Constructor that takes one argument.
    public Person(string nm)
    {
        name = nm;
    }

    // Method
    public void SetName(string newName)
    {
        name = newName;
    }
}
*/
