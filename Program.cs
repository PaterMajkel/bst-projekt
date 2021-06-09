using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Transactions;
using Priority_Queue;

namespace bst_projekt
{
    class Program
    {
        public class info2
        {
            public string date;
            public int id1;
            public int id2;
            //public int length;
            //public int speed;
            public int qid;
            public info2(int id1, int id2, 
                string date)
            {
                this.id1 = id1;
                this.id2 = id2;
                //this.length = length;
                //this.speed = speed;
                this.date = date;
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                qid = tab[0] * 10000 + tab[1] * 100 + tab[2];
            }
        }
        public class info
        {
            public string date;
            public int id1;
            public int id2;
            public int length;
            public int speed;
            public int qid;
            public info(int id1, int id2, int length, int speed,
                string date)
            {
                this.id1 = id1;
                this.id2 = id2;
                this.length = length;
                this.speed = speed;
                this.date = date;
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                qid = tab[0]*10000+tab[1]*100+tab[2];
            }
        }
        public class node
        {
            public node parent;
            public node Lchild;
            public node Rchild;
            public List<info> stations = new List<info>();
            public node(info Info)
            {
                stations.Add(Info);
            }


        }
        public class bstTree
        {
            public node root;
            public int count;
            public bstTree()
            {
                root = null;
                count = 0;
            }
            public void insert(int id1, int id2, int length, int speed,
                string date)
            {
                info insertion = new info(id1, id2, length, speed, date);
                node earlier=null;
                count++;
                node current = root;
                if(root==null)
                {
                    node newnode = new node(insertion);
                    newnode.parent = null;
                    root = newnode;
                    return;
                }
                while(current!=null)
                {
                    if (insertion.qid <= current.stations[0].qid)
                    {
                        if (insertion.qid == current.stations[0].qid)
                        {
                            //if(current.stations.IndexOf(insertion)==-1)
                                current.stations.Add(insertion);
                            return;
                        }
                        if(current.Lchild==null)
                        {
                            node newnode = new node(insertion);
                            newnode.parent = current;
                            current.Lchild = newnode;
                            return;
                        }
                        else
                            {
                            earlier = current;
                            current = current.Lchild;
                            }
                        
                    }
                    else
                    {
                        if (current.Rchild == null)
                        {
                            node newnode = new node(insertion);
                            newnode.parent = current;
                            current.Rchild = newnode;
                            return;
                        }
                        else
                        {
                            earlier = current;
                            current = current.Rchild;
                        }
                       // Console.WriteLine("bruh");
                        //System.Threading.Thread.Sleep(1000);
                    }
                        
                }
                
            }
            public bool delete(string date, int id1, int id2)
            {
                node deleted;
                node current=root;
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                int dateid = tab[0] * 10000 + tab[1] * 100 + tab[2];
                while (current!=null)
                {
                    if(current.stations[0].qid==dateid)
                    {
                        bool check = false;
                        deleted = current;
                        for(int i=0; i<current.stations.Count;i++)
                        {
                            if (current.stations[i].id1 == id1 && current.stations[i].id2 == id2 || current.stations[i].id1 == id2 && current.stations[i].id2 == id1)
                            {
                                current.stations.RemoveAt(i);
                                i--;
                                // Console.WriteLine("bruh1");
                                //System.Threading.Thread.Sleep(1000);
                                check = true;
                            }
                        }
                        if(current.stations.Count==0)
                        {
                            //Console.WriteLine("bruh2");
                            //System.Threading.Thread.Sleep(1000);
                            current = null;
                            traverse(deleted);
                            //return true;
                        }
                        if (check)
                        {
                            count--;
                            return true;
                        }
                        else
                            return false;
                        
                    }
                    else if (dateid < current.stations[0].qid)
                    {
                        current = current.Lchild;
                    }
                    else
                        current = current.Rchild;
                }
                return false;
            }
            public void traverse(node deleted)
            {
                node current=deleted;
                bool pom = true;
                if(current.Rchild!=null)
                {
                    current = current.Rchild;
                    if (current.Lchild != null)
                        while (current != null)
                        {
                            if (current.Lchild != null)
                                current = current.Lchild;
                            else
                            {
                                current.parent.Lchild = current.Rchild;
                                current.Lchild = deleted.Lchild;
                                current.Rchild = deleted.Rchild;
                                if (root == deleted)
                                {
                                    root = current;
                                    return;
                                }
                                if (deleted.parent.stations[0].qid < current.stations[0].qid)
                                {
                                    deleted.parent.Rchild = current;
                                    current.parent = deleted.parent;
                                }
                                else
                                {
                                    deleted.parent.Lchild = current;
                                    current.parent = deleted.parent;
                                }
                                return;
                            }
                        }
                    else
                    {
                        current.parent.Rchild = null;
                        if(deleted==root)
                        {
                            current.Lchild = deleted.Lchild;
                            root = current;
                            return;
                        }
                        if (deleted.parent.stations[0].qid < current.stations[0].qid)
                        {
                            deleted.parent.Rchild = current;
                            current.Lchild = deleted.Lchild;
                            current.parent = deleted.parent;
                        }
                        else
                        {
                            deleted.parent.Lchild = current;
                            current.Lchild = deleted.Lchild;
                            current.parent = deleted.parent;
                        }
                        return;
                    }
                }
                else if (current.Lchild != null)
                {
                    current = current.Lchild;
                    if (current.Rchild != null)
                        while (current != null)
                        {
                            if (current.Rchild != null)
                                current = current.Rchild;
                            else
                            {
                                current.parent.Rchild = current.Lchild;
                                current.Rchild = deleted.Rchild;
                                current.Lchild = deleted.Lchild;
                                if (root == deleted)
                                {
                                    root = current;
                                    return;
                                }
                                if (deleted.parent.stations[0].qid < current.stations[0].qid)
                                {
                                    deleted.parent.Rchild = current;
                                    current.parent = deleted.parent;
                                }
                                else
                                {
                                    deleted.parent.Lchild = current;
                                    current.parent = deleted.parent;
                                }



                                return;

                            }
                        }
                    else
                    {
                        current.parent.Lchild = null;
                        if (deleted == root)
                        {
                            current.Rchild = deleted.Rchild;
                            root = current;
                            return;
                        }
                        if (deleted.parent.stations[0].qid < current.stations[0].qid)
                        {
                            deleted.parent.Rchild = current;
                            current.Rchild = deleted.Rchild;
                            current.parent = deleted.parent;
                        }
                        else
                        {
                            deleted.parent.Lchild = current;
                            current.Rchild = deleted.Rchild;
                            current.parent = deleted.parent;
                        }
                        return;
                    }

                }
                else
                {
                    if (current == root)
                    {
                        root = null;
                        return;
                    }
                    if (current.parent.Lchild == current)
                        current.parent.Lchild = null;
                    else
                        current.parent.Rchild = null;
                    return;
                    
                }
                
            }
            public void write(node curr, int count=0, bool pom=true)
            {
                if (curr == null || curr.stations.Count==0)
                    return;
                if(!pom)
                {
                    for (int i = 0; i < count; i++)
                        Console.Write("\t      .");
                }
                if (count == 0)
                    curr = root;
                Console.Write(curr.stations[0].date+"("+curr.stations.Count+")"+"\t");
                write(curr.Rchild, ++count);
                Console.WriteLine();
                write(curr.Lchild, ++count, false);
                return;
            }

            public int connectionCount(node curr,int date1, int date2, bool check=true)//2000-12-12 2051-12-12
            {

                int count = 0;
                if (curr == null || curr.stations.Count == 0)
                    return 0;

                if (count == 0 && check)
                    curr = root;
                //if(curr.stations[0].qid<date1)
                //{
                //    count += curr.stations.Count;
                //}
                if (curr.stations[0].qid <= date2 && curr.stations[0].qid >= date1)
                {
                    count += curr.stations.Count;
                }
                if (curr.stations[0].qid <= date2)
                    count +=connectionCount(curr.Rchild, date1,date2,false);
                if(curr.stations[0].qid >= date1)
                    count+=connectionCount(curr.Lchild,date1, date2,false);
                return count;
            }
            public bool search(string date, int id1, int id2, bool whether=false)
            {
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                int dateid = tab[0] * 10000 + tab[1] * 100 + tab[2];
                node current = root;
                while(current!=null)
                {
                    if (current.stations[0].qid == dateid)
                    {
                        for (int i = 0; i < current.stations.Count; i++)
                        {
                            if (current.stations[i].id1 == id1 && current.stations[i].id2 == id2 ||
                                current.stations[i].id2 == id1 && current.stations[i].id1 == id2)
                            {
                                if (whether)
                                {
                                    for (int j = 0; j < current.stations.Count; j++)
                                    {
                                        Console.Write(current.stations[j].date + "," + current.stations[j].id1 + "," + current.stations[j].id2);
                                    }
                                    Console.WriteLine();
                                }
                                return true;
                            }
                        }
                        break;
                    }
                        
                    else if (dateid < current.stations[0].qid)
                    {
                        current = current.Lchild;
                    }
                    else if (dateid > current.stations[0].qid)
                        current = current.Rchild;
                    else
                        return false;
                    
                }
                return false;
            }
        }
        public class gotoStation 
        {
            public int idStation;
            public List<int> times;
            public List<int> qids;
            public List<string> dates;

            public gotoStation(int id, string date, int time)
            {
                qids = new List<int>();
                times = new List<int>();
                dates = new List<string>();
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                qids.Add(tab[0] * 10000 + tab[1] * 100 + tab[2]);
                idStation = id;
                dates.Add(date);
                times.Add(time);
            }
            public void addToExisting(string date, int time)
            {
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                int pom = tab[0] * 10000 + tab[1] * 100 + tab[2];
                for(int i=times.Count-1; i>=0; i--)
                {
                    if(pom>=qids[i])
                    {
                        qids.Insert(i+1, pom);
                        times.Insert(i+1, time);
                        dates.Insert(i+1, date);
                        return;
                    }
                }
                qids.Insert(0, pom);
                times.Insert(0, time);
                dates.Insert(0, date);
                return;
            }
        }
        public class startingPoint
        {
            public string date;
            public int qid;
            public int idStation;
            public startingPoint(int id, string date)
            {
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                qid = tab[0] * 10000 + tab[1] * 100 + tab[2];
                idStation = id;
                this.date = date;
            }
        }
        /*public class graphonDIC
        {
            List<int> dateList;
            Dictionary<int, Dictionary<int, gotoStation>> neighboursDIC;
            public graphonDIC()
            {
                neighboursDIC = new Dictionary<int, Dictionary<int, gotoStation>>();
                dateList = new List<int>();
            }
            public void insert(int id1, int id2, int length, int speed,
                string date)
            {
                int time = 60 * length / speed;
                //startingPoint temp1 = new startingPoint(id1, date);
                if (!neighboursDIC.ContainsKey(id1))
                {
                    neighboursDIC.Add(id1, new Dictionary<int, gotoStation>());
                    neighboursDIC[id1].Add(id2, new gotoStation(id2, date, time));
                }
                else
                {
                    if(neighboursDIC[id1].ContainsKey(id2))
                    {
                        neighboursDIC[id1][id2].addToExisting(date, time);
                    }
                    else
                    {
                        neighboursDIC[id1].Add(id2, new gotoStation(id2, date, time));
                    }

                }
                if (!neighboursDIC.ContainsKey(id2))
                {
                    neighboursDIC.Add(id2, new Dictionary<int, gotoStation>());
                    neighboursDIC[id2].Add(id1, new gotoStation(id1, date, time));
                }
                else
                {
                    if (neighboursDIC[id2].ContainsKey(id1))
                    {
                        neighboursDIC[id2][id1].addToExisting(date, time);
                    }
                    else
                    {
                        neighboursDIC[id2].Add(id1, new gotoStation(id1, date, time));
                    }

                }
                int[] tab1 = Array.ConvertAll(date.Split('-'), int.Parse);
                int qid = tab1[0] * 10000 + tab1[1] * 100 + tab1[2];
                dateList.Add(qid);


            }
            public void delete(int id1, int id2, string date, bstTree tree)
            {
                if(neighboursDIC.TryGetValue(id1,out Dictionary<int, gotoStation> idPrawe))
                {
                    if(idPrawe.TryGetValue(id2,out gotoStation station))
                    {
                        int index=station.dates.IndexOf(date);
                        if(index>-1)
                        {
                            station.dates.RemoveAt(index);
                            station.qids.RemoveAt(index);
                            station.times.RemoveAt(index);
                            if (station.dates.Count == 0)
                                neighboursDIC[id1].Remove(id2);

                        }
                    }
                    if(neighboursDIC[id1].Count==0)
                    {
                        neighboursDIC.Remove(id1);
                    }
                }
                if (neighboursDIC.TryGetValue(id2, out Dictionary<int, gotoStation> idLewe))
                {
                    if (idLewe.TryGetValue(id2, out gotoStation station))
                    {
                        int index = station.dates.IndexOf(date);
                        if (index > -1)
                        {
                            station.dates.RemoveAt(index);
                            station.qids.RemoveAt(index);
                            station.times.RemoveAt(index);
                            if (station.dates.Count == 0)
                                neighboursDIC[id2].Remove(id1);

                        }
                    }
                    if (neighboursDIC[id2].Count == 0)
                    {
                        neighboursDIC.Remove(id2);
                    }
                }


                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                int qid = tab[0] * 10000 + tab[1] * 100 + tab[2];
                if (tree.connectionCount(tree.root, qid, qid) < 1)
                    dateList.Remove(qid);
            }
            public void write()
            {
                foreach (var x in neighboursDIC)
                {
                    Console.WriteLine(x.Key + ":");
                    foreach (var y in neighboursDIC[x.Key])
                    {
                        Console.Write("\t" + y.Key + "(");
                        for (int i = 0; i < y.Value.dates.Count; i++)
                        {
                            Console.Write(y.Value.times[i] + "," + y.Value.dates[i] + " ");
                        }
                        Console.WriteLine(")");
                    }
                    Console.WriteLine();

                }
            }
            public int dijktra_priorityWAR1(int idStart, int idFinish)
            {
                Dictionary<int, int> distance = new Dictionary<int, int>();
                Dictionary<int, bool> visited = new Dictionary<int, bool>();
                Dictionary<int, int> from = new Dictionary<int, int>();
                SimplePriorityQueue<int> queue = new SimplePriorityQueue<int>();

                foreach(var x in neighboursDIC)
                {
                    if (x.Key == idStart)
                        queue.Enqueue(idStart, 0);
                    else
                    {
                        queue.Enqueue(x.Key, int.MaxValue);
                        distance.Add(x.Key, int.MaxValue);
                        visited.Add(x.Key, false);
                    }
                }
                try
                {
                    distance[idStart] = 0;

                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }


                while (queue.Count > 0)
                {
                    int help = queue.Dequeue();
                    visited[help] = true;
                    if (help == idFinish)
                    {
                        return distance[idFinish] - 5;
                    }
                    foreach (var neigh in neighboursDIC[help])
                    {
                        int cost = neigh.Value.times[neigh.Value.times.Count - 1] + 5;
                        if (distance[help] + cost < distance[neigh.Key])
                        {
                            distance[neigh.Key] = distance[help] + cost;
                            from[neigh.Key] = help;
                            if(visited[neigh.Key]==false)
                                queue.Enqueue(neigh.Key, distance[neigh.Key]);
                        }
                    }

                }
                return 0;
            }
            public int dijkstra_priority_queueWAR2(int idStart, int idFinish, int limit = 0)
            {
                dateList.Sort();
                int mindate = int.MaxValue;
                List<int> datelist2 = new List<int>();
                foreach (var x in neighboursDIC[idFinish])
                {
                    foreach (var y in x.Value.qids)
                    {
                        if (y < mindate)
                            mindate = y;
                        datelist2.Add(y);
                    }
                }
                datelist2.Sort();
                return dijkstra_binary_search(idStart, idFinish, datelist2[0], dateList[dateList.Count - 1], limit);
            }

            public int dijkstra_binary_search(int idStart, int idFinish, int mindate, int maksdate, int limit = 0)
            {
                //dateList.Sort();
                int bin2 = dateList.IndexOf(maksdate);
                int bin1 = dateList.IndexOf(mindate);
                int diff = bin2 - bin1;
                if (diff < 2)
                {
                    return mindate;
                }
                else
                {
                    int wynik = dijktra_priorityWAR1(idStart, idFinish);
                    if (wynik > limit)
                        return dijkstra_binary_search(idStart, idFinish, dateList[bin1 + diff / 2], maksdate, limit);
                    else
                        return dijkstra_binary_search(idStart, idFinish, mindate, dateList[bin2 - diff / 2 + 1], limit);
                }
            }
        }*/
        public class graph
        {
            //public List<startingPoint> startingPoints;
            public List<int> startingPoints;
            public List<List<gotoStation>> neighbours;
            List<int> dateList;

            public graph()
            {
                neighbours = new List<List<gotoStation>>();
                startingPoints = new List<int>();
                dateList = new List<int>();
            }
            public void insert(int id1, int id2, int length, int speed,
                string date)
            {
                int time = 60 * length / speed;
                //startingPoint temp1 = new startingPoint(id1, date);
                if (!startingPoints.Contains(id1))
                {
                    startingPoints.Add(id1);
                    neighbours.Add(new List<gotoStation>());
                    neighbours[startingPoints.Count - 1].Add(new gotoStation(id2, date, time));
                }
                else
                {
                    int pom = startingPoints.IndexOf(id1);
                    bool check = true;
                    for (int i = 0; i < neighbours[pom].Count; i++)
                    {
                        if (neighbours[pom][i].idStation == id2)
                        {
                            neighbours[pom][i].addToExisting(date, time);
                            check = false;
                            break;
                        }

                    }
                    if (check)
                        neighbours[pom].Add(new gotoStation(id2, date, time));
                }
                //startingPoint temp2 = new startingPoint(id2, date);
                if (!startingPoints.Contains(id2))
                {
                    startingPoints.Add(id2);
                    neighbours.Add(new List<gotoStation>());
                    neighbours[startingPoints.Count - 1].Add(new gotoStation(id1, date, time));
                }
                else
                {
                    int pom = startingPoints.IndexOf(id2);
                    bool check = true;
                    for (int i = 0; i < neighbours[pom].Count; i++)
                    {
                        if (neighbours[pom][i].idStation == id1)
                        {
                            neighbours[pom][i].addToExisting(date, time);
                            check = false;
                            break;
                        }

                    }
                    if (check)
                        neighbours[pom].Add(new gotoStation(id1, date, time));
                }
                int[] tab1 = Array.ConvertAll(date.Split('-'), int.Parse);
                int qid = tab1[0] * 10000 + tab1[1] * 100 + tab1[2];
                dateList.Add(qid);


            }
            public void delete(int id1, int id2, string date, bstTree tree)
            {
                int pom = startingPoints.IndexOf(id1);
                for (int i = 0; i < neighbours[pom].Count; i++)
                {
                    if (neighbours[pom][i].idStation == id2)
                    {
                        int pom2 = neighbours[pom][i].dates.IndexOf(date);
                        if (pom2 == -1)
                            continue;
                        neighbours[pom][i].dates.RemoveAt(pom2);
                        neighbours[pom][i].qids.RemoveAt(pom2);
                        neighbours[pom][i].times.RemoveAt(pom2);
                        if (neighbours[pom][i].qids.Count == 0)
                        {
                            neighbours[pom].RemoveAt(i);
                            i--;
                        }
                        if (neighbours[pom].Count == 0)
                        {
                            neighbours.RemoveAt(pom);
                            startingPoints.RemoveAt(pom);
                        }
                        break;
                    }
                }
                pom = startingPoints.IndexOf(id2);
                for (int i = 0; i < neighbours[pom].Count; i++)
                {
                    if (neighbours[pom][i].idStation == id1)
                    {
                        int pom2 = neighbours[pom][i].dates.IndexOf(date);
                        if (pom2 == -1)
                            continue;
                        neighbours[pom][i].dates.RemoveAt(pom2);
                        neighbours[pom][i].qids.RemoveAt(pom2);
                        neighbours[pom][i].times.RemoveAt(pom2);
                        if (neighbours[pom][i].qids.Count == 0)
                        {
                            neighbours[pom].RemoveAt(i);
                            i--;
                        }
                        if (neighbours[pom].Count == 0)
                        {
                            neighbours.RemoveAt(pom);
                            startingPoints.RemoveAt(pom);
                        }
                        break;
                    }
                }
                int[] tab = Array.ConvertAll(date.Split('-'), int.Parse);
                int qid = tab[0] * 10000 + tab[1] * 100 + tab[2];
                if (tree.connectionCount(tree.root, qid, qid) < 1)
                    dateList.Remove(qid);
            }
            public void write()
            {
                for (int i = 0; i < startingPoints.Count; i++)
                {
                    Console.Write(startingPoints[i] + ":\t");
                    Console.WriteLine();
                    for (int j = 0; j < neighbours[i].Count; j++)
                    {
                        Console.Write("\t" + neighbours[i][j].idStation + "(");
                        for (int k = 0; k < neighbours[i][j].qids.Count; k++)
                            Console.Write(neighbours[i][j].times[k] + "," + neighbours[i][j].dates[k] + " ");

                        Console.WriteLine(")");
                    }
                    Console.WriteLine();
                }
            }
            public int dijktra(int idStart, int idFinish)
            {
                int[] distance = new int[startingPoints.Count];
                bool[] visited = new bool[startingPoints.Count];
                int[] from = new int[startingPoints.Count];
                for (int i = 0; i < distance.Length; i++)
                    distance[i] = int.MaxValue;
                try
                {
                    distance[startingPoints.IndexOf(idStart)] = 0;

                }
                catch(IndexOutOfRangeException)
                {
                    return 0;
                }
                for (int i = 0; i < distance.Length; i++)
                {
                    int pom = int.MaxValue;
                    int current = 0;

                    for (int j = 0; j < distance.Length; j++)
                    {
                        if (distance[j] <= pom && !visited[j])
                        {
                            if (i > 0 && distance[j] == 0)
                                continue;
                            current = j;
                            pom = distance[j];
                        }

                    }
                    visited[current] = true;
                    foreach (var x in neighbours[current])
                    {
                        int cost = x.times[x.times.Count - 1] + 5;

                        int help = startingPoints.IndexOf(x.idStation);
                        if (distance[current] + cost < distance[help])
                        {
                            distance[help] = distance[current] + cost;
                            from[help] = current;
                        }

                        //if (d[v] + waga(v, w) < d[w])
                        //   d[w] = d[v] + waga(v, w)
                        //   p[w] = v
                    }

                }
                int temp = startingPoints.IndexOf(idFinish);
                return distance[temp] - 5;
            }
            public int dijktra2(int idStart, int idFinish, int limit = 0)
            {
                dateList.Sort();
                int maksdate = 0;
                int mindate = int.MaxValue;
                foreach (var x in neighbours[startingPoints.IndexOf(idFinish)])
                {
                    foreach (var y in x.qids)
                    {
                        if (y < mindate)
                            mindate = y;
                    }
                }
                //dateList.Add(18531115);
                /*foreach (var x in neighbours[startingPoints.IndexOf(idStart)])
                {
                    foreach (var y in x.qids)
                    {
                        if (dateList.IndexOf(y) == -1)
                            dateList.Add(y);
                    }
                }*/
                for (int a = dateList.IndexOf(mindate); a < dateList.Count; a++)
                {
                    int[] distance = new int[startingPoints.Count];
                    bool[] visited = new bool[startingPoints.Count];
                    int[] from = new int[startingPoints.Count];
                    for (int i = 0; i < distance.Length; i++)
                        distance[i] = int.MaxValue;
                    distance[startingPoints.IndexOf(idStart)] = 0;
                    int temp = startingPoints.IndexOf(idFinish);
                    from[startingPoints.IndexOf(idStart)] = 0;
                    for (int i = 0; i < distance.Length - 1; i++)
                    {
                        int pom = int.MaxValue;
                        int current = 0;

                        for (int j = 0; j < distance.Length; j++)
                        {
                            if (distance[j] <= pom && !visited[j])
                            {
                                current = j;
                                pom = distance[j];
                            }

                        }
                        if (pom == int.MaxValue)
                            break;
                        visited[current] = true;

                        foreach (var x in neighbours[current])
                        {
                            int cost = int.MaxValue;
                            int currentTime = 0;
                            for (int j = x.qids.Count - 1; j >= 0; j--)
                            {
                                if (x.qids[j] <= dateList[a])
                                {
                                    cost = x.times[j] + 5;
                                    currentTime = x.qids[j];
                                    break;
                                }
                            }

                            int help = startingPoints.IndexOf(x.idStation); //id wierzcholka sasiada
                            if (distance[current] + cost < distance[help] && cost != int.MaxValue)
                            {
                                distance[help] = distance[current] + cost;
                                from[help] = current;

                                if (distance[temp] - 5 <= limit)
                                {
                                    Console.Write(idStart + " F" + idFinish + " D" + distance[temp] + " L" + limit + " ");
                                    return dateList[a];

                                }
                            }

                            //if (d[v] + waga(v, w) < d[w])
                            //   d[w] = d[v] + waga(v, w)
                            //   p[w] = v
                        }

                    }
                    if (distance[temp] - 5 <= limit)
                        return dateList[a];


                }
                return -1;

            }
            public int dijkstra3(int idStart, int idFinish, int limit = 0)
            {
                dateList.Sort();
                int mindate = int.MaxValue;
                List<int> datelist2 = new List<int>();
                foreach (var x in neighbours[startingPoints.IndexOf(idFinish)])
                {
                    foreach (var y in x.qids)
                    {
                        if (y < mindate)
                            mindate = y;
                        datelist2.Add(y);
                    }
                }
                datelist2.Sort();
                return dijkstra_binary_search(idStart, idFinish, datelist2[0], dateList[dateList.Count - 1], limit);
            }
            public int dijktra_priority(int idStart, int idFinish)
            {
                int[] distance = new int[startingPoints.Count];
                bool[] visited = new bool[startingPoints.Count];
                int[] from = new int[startingPoints.Count];
                SimplePriorityQueue<int> queue = new SimplePriorityQueue<int>();


                for (int i = 0; i < distance.Length; i++)
                {
                    if (startingPoints[i] == idStart)
                        queue.Enqueue(idStart, 0);
                    else
                    {
                        queue.Enqueue(startingPoints[i], int.MaxValue);
                        distance[i] = int.MaxValue;

                    }

                }
                try
                {
                    distance[startingPoints.IndexOf(idStart)] = 0;

                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }


                while(queue.Count>0)
                {
                    int help = queue.Dequeue();
                    if (help == idFinish)
                    {
                        return distance[startingPoints.IndexOf(idFinish)]-5;
                    }
                    int current = startingPoints.IndexOf(help);
                    foreach(var neigh in neighbours[current])
                    {
                        int cost=neigh.times[neigh.times.Count - 1] + 5;
                        int pom= startingPoints.IndexOf(neigh.idStation);
                        if(distance[current]+cost<distance[pom])
                        {
                            distance[pom] = distance[current] + cost;
                            from[pom] = current;
                            queue.Enqueue(neigh.idStation, distance[startingPoints.IndexOf(neigh.idStation)]);
                        }
                    }

                }



                return 0;
            }
            public int dijkstra_get_points(int idStart, int idFinish, int limit = 0)
            {
                dateList.Sort();
                List<int> datelist2 = new List<int>();
                int maksdate = 0;
                int mindate = int.MaxValue;
                foreach (var x in neighbours[startingPoints.IndexOf(idStart)])
                {
                    foreach (var y in x.qids)
                    {
                        if (y < mindate)
                            mindate = y;
                        datelist2.Add(y);
                    }
                }
                datelist2.Sort();
                for (int a = 0; a < datelist2.Count; a++)
                {
                    int[] distance = new int[startingPoints.Count];
                    bool[] visited = new bool[startingPoints.Count];
                    int[] from = new int[startingPoints.Count];
                    for (int i = 0; i < distance.Length; i++)
                        distance[i] = int.MaxValue;
                    distance[startingPoints.IndexOf(idStart)] = 0;
                    int temp = startingPoints.IndexOf(idFinish);
                    from[startingPoints.IndexOf(idStart)] = 0;
                    for (int i = 0; i < distance.Length - 1; i++)
                    {
                        int pom = int.MaxValue;
                        int current = 0;

                        for (int j = 0; j < distance.Length; j++)
                        {
                            if (distance[j] <= pom && !visited[j])
                            {
                                current = j;
                                pom = distance[j];
                            }

                        }
                        if (pom == int.MaxValue)
                            break;
                        visited[current] = true;

                        foreach (var x in neighbours[current])
                        {
                            int cost = int.MaxValue;
                            int currentTime = 0;
                            for (int j = x.qids.Count - 1; j >= 0; j--)
                            {
                                if (x.qids[j] <= datelist2[a])
                                {
                                    cost = x.times[j] + 5;
                                    currentTime = x.qids[j];
                                    break;
                                }
                            }

                            int help = startingPoints.IndexOf(x.idStation); //id wierzcholka sasiada
                            if (distance[current] + cost < distance[help] && cost != int.MaxValue)
                            {
                                distance[help] = distance[current] + cost;
                                from[help] = current;

                                if (distance[temp] - 5 <= limit)
                                {
                                    //Console.Write(idStart + " F" + idFinish + " D" + distance[temp] + " L" + limit + " ");
                                    if (a > 0)
                                        return dijkstra_binary_search(idStart, idFinish, datelist2[a - 1], datelist2[a], limit);
                                    else
                                        return dijkstra_binary_search(idStart, idFinish, int.MaxValue, datelist2[a], limit);
                                    /*   
                                    if (a > 0)
                                       return dijkstra_gotten(idStart, idFinish, datelist2[a-1], datelist2[a], limit);
                                    else
                                        return dijkstra_gotten(idStart, idFinish,datelist2[a], datelist2[a], limit);
                                        */
                                }
                            }

                            //if (d[v] + waga(v, w) < d[w])
                            //   d[w] = d[v] + waga(v, w)
                            //   p[w] = v
                        }

                    }
                    if (distance[temp] - 5 <= limit)
                        return datelist2[a];


                }
                return -1;

            }





            public int dijkstra_gotten(int idStart, int idFinish, int mindate, int maksdate, int limit = 0)
            {
                //dateList.Sort();


                for (int a = dateList.IndexOf(mindate); a <= dateList.IndexOf(maksdate); a++)
                {
                    int[] distance = new int[startingPoints.Count];
                    bool[] visited = new bool[startingPoints.Count];
                    int[] from = new int[startingPoints.Count];
                    for (int i = 0; i < distance.Length; i++)
                        distance[i] = int.MaxValue;
                    distance[startingPoints.IndexOf(idStart)] = 0;
                    int temp = startingPoints.IndexOf(idFinish);
                    from[startingPoints.IndexOf(idStart)] = 0;
                    for (int i = 0; i < distance.Length - 1; i++)
                    {
                        int pom = int.MaxValue;
                        int current = 0;

                        for (int j = 0; j < distance.Length; j++)
                        {
                            if (distance[j] <= pom && !visited[j])
                            {
                                current = j;
                                pom = distance[j];
                            }

                        }
                        if (pom == int.MaxValue)
                            break;
                        visited[current] = true;

                        foreach (var x in neighbours[current])
                        {
                            int cost = int.MaxValue;
                            int currentTime = 0;
                            for (int j = x.qids.Count - 1; j >= 0; j--)
                            {
                                if (x.qids[j] <= dateList[a])
                                {
                                    cost = x.times[j] + 5;
                                    currentTime = x.qids[j];
                                    break;
                                }
                            }

                            int help = startingPoints.IndexOf(x.idStation); //id wierzcholka sasiada
                            if (distance[current] + cost < distance[help] && cost != int.MaxValue)
                            {
                                distance[help] = distance[current] + cost;
                                from[help] = current;

                                if (distance[temp] - 5 <= limit)
                                {
                                    //Console.Write(idStart + " F" + idFinish + " D" + distance[temp] + " L" + limit + " ");
                                    return dateList[a];

                                }
                            }

                            //if (d[v] + waga(v, w) < d[w])
                            //   d[w] = d[v] + waga(v, w)
                            //   p[w] = v
                        }

                    }
                    if (distance[temp] - 5 <= limit)
                        return dateList[a];


                }
                return -1;

            }
            public int dijkstra_binary_search(int idStart, int idFinish, int mindate, int maksdate, int limit = 0)
            {
                //dateList.Sort();
                int bin2 = dateList.IndexOf(maksdate);
                int bin1 = dateList.IndexOf(mindate);
                int diff = bin2 - bin1;
                if (diff <5)
                {
                    return dijkstra_gotten(idStart, idFinish, mindate, maksdate, limit);
                }
                else
                {
                    int[] distance = new int[startingPoints.Count];
                    bool[] visited = new bool[startingPoints.Count];
                    int[] from = new int[startingPoints.Count];
                    for (int i = 0; i < distance.Length; i++)
                        distance[i] = int.MaxValue;
                    distance[startingPoints.IndexOf(idStart)] = 0;
                    int temp = startingPoints.IndexOf(idFinish);
                    from[startingPoints.IndexOf(idStart)] = 0;
                    for (int i = 0; i < distance.Length - 1; i++)
                    {
                        int pom = int.MaxValue;
                        int current = 0;

                        for (int j = 0; j < distance.Length; j++)
                        {
                            if (distance[j] <= pom && !visited[j])
                            {
                                current = j;
                                pom = distance[j];
                            }

                        }
                        if (pom == int.MaxValue)
                            break;
                        visited[current] = true;

                        foreach (var x in neighbours[current])
                        {
                            int cost = int.MaxValue;
                            int currentTime = 0;
                            for (int j = x.qids.Count - 1; j >= 0; j--)
                            {
                                if (x.qids[j] <= dateList[bin2-diff/2+1])
                                {
                                    cost = x.times[j] + 5;
                                    currentTime = x.qids[j];
                                    break;
                                }
                            }

                            int help = startingPoints.IndexOf(x.idStation); //id wierzcholka sasiada
                            if (distance[current] + cost < distance[help] && cost != int.MaxValue)
                            {
                                distance[help] = distance[current] + cost;
                                from[help] = current;

                                if (distance[temp] - 5 <= limit)
                                {
                                    //Console.Write(idStart + " F" + idFinish + " D" + distance[temp] + " L" + limit + " ");
                                    
                                    return dijkstra_binary_search(idStart, idFinish, mindate, dateList[bin2 - diff / 2 + 1], limit);

                                }
                            }

                            //if (d[v] + waga(v, w) < d[w])
                            //   d[w] = d[v] + waga(v, w)
                            //   p[w] = v
                        }
                        

                    }
                    //return dijkstra_binary_search(idStart, idFinish, mindate, maksdate - diff / 2 + 1, limit);
                    return dijkstra_binary_search(idStart, idFinish, dateList[bin1 + diff / 2], maksdate, limit);

                }
                return -1;
            }
        

        }
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //info proba = new info(14, 21, 21, 20, "2000-07-19");
            //Console.WriteLine(proba.qid);
            //Console.ReadLine();
            bstTree tree = new bstTree();
            graph Graph = new graph();
            //graphonDIC GraphDIC = new graphonDIC();
            List<info2> amIretarded = new List<info2>();
            StreamWriter streamWriter = new StreamWriter("Niedzialajace usuwania.txt");
            int option = -1;
            while (option != 0)
            {
                Console.Clear();
                Console.WriteLine("1.Dodaj\n2.Usun\n3.Wypisz\n4.Wypisz graf\n5.Przykladowe dane\n6.Wyszukaj\n7.Ile stacji wybudowanych w danym czasie\n" +
                    "8.Najkrotsza sciezka\n9.Najkrotsza sciezka przed pewna data\n10.Pobierz z pliku");
                option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        {
                            Console.WriteLine("Podaj id pierwszej stacji, drugiej stacji, predkosc pociagu, dlugosc drogi i date wybudowania");
                            string[] tab = Console.ReadLine().Trim().Split();
                            tree.insert(int.Parse(tab[0]), int.Parse(tab[1]), int.Parse(tab[3]), int.Parse(tab[2]),tab[4]);
                            Graph.insert(int.Parse(tab[0]), int.Parse(tab[1]), int.Parse(tab[3]), int.Parse(tab[2]), tab[4]);
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Podaj date i id pierwszej oraz drugiej stacji do usuniecia ");
                            string[] pom = Console.ReadLine().Trim().Split();
                            if(tree.delete(pom[0], int.Parse(pom[1]), int.Parse(pom[2])))
                            {
                                Console.WriteLine("Wartosc zostala usunieta");
                                Graph.delete(int.Parse(pom[1]), int.Parse(pom[2]), pom[0], tree);
                            }
                            else
                                Console.WriteLine("Brak takiej wartosci");
                            Console.ReadLine();
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            tree.write(tree.root);
                            Console.ReadLine();
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            Graph.write();
                            Console.ReadLine();
                            break;
                        }
                    case 5:
                        {
                            tree.insert(1, 2, 450, 422, "2000-02-18"); //2000-02-18 1 2
                            Graph.insert(1, 2, 450, 422, "2000-02-18");

                            tree.insert(8, 10, 450, 422, "2001-02-18"); //2001-02-18 8 10
                            Graph.insert(8, 10, 450, 422, "2001-02-18");

                            tree.insert(10, 24, 450, 422, "1999-02-18");//1999-02-18 141 228
                            Graph.insert(10, 24, 450, 422, "1999-02-18");

                            tree.insert(10, 81, 450, 422, "1976-02-18");//1976-02-18 10 81
                            Graph.insert(10, 81, 450, 422, "1976-02-18");

                            tree.insert(2, 24, 450, 422, "1950-02-18");//1950-02-18 2 24
                            Graph.insert(2, 24, 450, 422, "1950-02-18");

                            tree.insert(1, 81, 450, 422, "2021-02-18");//2021-02-18 1 81
                            Graph.insert(1, 81, 450, 422, "2021-02-18");

                            tree.insert(24, 8, 450, 422, "2050-02-18");//2050-02-18 24 8
                            Graph.insert(24, 8, 450, 422, "2050-02-18");

                            tree.insert(43, 12, 450, 422, "2005-02-18");//2005-02-18 43 12
                            Graph.insert(43, 12, 450, 422, "2005-02-18");

                            tree.insert(12, 8, 450, 422, "1998-02-18");//1998-02-18 12 8
                            Graph.insert(12, 8, 450, 422, "1998-02-18");

                            tree.insert(12, 8, 450, 422, "1999-02-18");//1998-02-18 12 8
                            Graph.insert(12, 8, 450, 422, "1999-02-18");
                            tree.insert(12, 8, 450, 422, "1901-02-18");//1998-02-18 12 8
                            Graph.insert(12, 8, 450, 422, "1901-02-18");
                            tree.insert(12, 8, 450, 422, "2051-02-18");//1998-02-18 12 8
                            Graph.insert(12, 8, 450, 422, "2051-02-18");
                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("Podaj date, id1, id2 do znalezienia");
                            string[] pom = Console.ReadLine().Trim().Split();
                            if (!tree.search(pom[0], int.Parse(pom[1]), int.Parse(pom[2]),pom.Length>3))
                                Console.WriteLine("Brak wartosci");
                            else
                                Console.WriteLine("Istnieje");
                            Console.ReadLine();
                            break;
                        }
                    case 7:
                        {
                            Console.WriteLine("Podaj pierwsza i druga date");
                            string[] pom = Console.ReadLine().Trim().Split();
                            int[] tab1 = Array.ConvertAll(pom[0].Split('-'), int.Parse);
                            int qid1 = tab1[0] * 10000 + tab1[1] * 100 + tab1[2];
                            int[] tab2 = Array.ConvertAll(pom[1].Split('-'), int.Parse);
                            int qid2 = tab2[0] * 10000 + tab2[1] * 100 + tab2[2];
                            if(qid2<qid1)
                            {
                                int swap = qid1;
                                qid1 = qid2;
                                qid2 = swap;
                            }
                            //Console.WriteLine(tree.count +" "+ tree.connectionCount(tree.root, qid1, qid2));
                            Console.WriteLine(/*tree.count -*/ tree.connectionCount(tree.root, qid1, qid2));
                            Console.ReadLine();
                            break;
                        }
                    case 8:
                        {
                            Console.WriteLine("Podaj pierwsza i druga stacje");
                            int[] pom = Array.ConvertAll(Console.ReadLine().Trim().Split(),int.Parse);
                            Console.WriteLine(Graph.dijktra(pom[0],pom[1]));
                            Console.ReadLine();
                            break;
                        }
                    case 9:
                        {
                            Console.WriteLine("Podaj pierwsza i druga stacje i limit");
                            string[] pom = Console.ReadLine().Trim().Split();
                            //int[] tab = Array.ConvertAll(pom[2].Split('-'), int.Parse);
                            //int qid = tab[0] * 10000 + tab[1] * 100 + tab[2];
                            Console.WriteLine(Graph.dijktra2(int.Parse(pom[0]), int.Parse(pom[1]),int.Parse(pom[2])));
                            Console.ReadLine();
                            break;
                        }
                    case 10:
                        {
                            
                            StreamReader plik = new StreamReader("projekt2_in1.txt");
                            StreamReader plik2 = new StreamReader("projekt2_out1.txt");
                            string count = plik.ReadLine();
                            int sumy = 0;
                            int roznice = 0;
                            int wszystkie_roznice = 0;
                            for (int i = 0; i < int.Parse(count.Trim()); i++)
                            {
                                string[] opt = plik.ReadLine().Trim().Split();
                                //if (i == 80)
                                 //   ;
                                switch (opt[0])
                                {
                                    
                                    case "DP"://dodaj
                                        {
                                            
                                            tree.insert(int.Parse(opt[2]), int.Parse(opt[3]), int.Parse(opt[5]), int.Parse(opt[4]), opt[1]);//id1, id2, predkosc, droga, data
                                            Graph.insert(int.Parse(opt[2]), int.Parse(opt[3]), int.Parse(opt[5]), int.Parse(opt[4]), opt[1]);
                                            //amIretarded.Add(new info2(int.Parse(opt[3]), int.Parse(opt[2]), opt[1]));
                                            sumy++;
                                            break;
                                        }
                                    case "UP"://usun
                                        {
                                            //tree.write(tree.root);
                                            //bool nugget = false;
                                            //bool nugget2 = false;
                                            //int bruh = amIretarded.IndexOf(new info2(int.Parse(opt[2]), int.Parse(opt[3]), opt[1]));
                                            //int bruh2 = amIretarded.IndexOf(new info2(int.Parse(opt[3]), int.Parse(opt[2]), opt[1]));
                                            //if (bruh > -1)
                                            //{
                                            //    amIretarded.RemoveAt(bruh);
                                            //    nugget2 = true;
                                            //}
                                            //if (bruh2 > -1)
                                            // {
                                            //    amIretarded.RemoveAt(bruh2);
                                            //    nugget2 = true;
                                            //}
                                            //tree.write(tree.root);

                                            //if (opt[1] == "1560-04-03" && opt[2] == "12" && opt[3] == "20")
                                            //   ;
                                            //int z = 0;
                                            //if(tree.search(opt[1], int.Parse(opt[2]), int.Parse(opt[3]), false))
                                               // z=tree.connectionCount(tree.root, 10000000, 99999999);

                                            //bool bruh = false;
                                            if (tree.delete(opt[1], int.Parse(opt[2]), int.Parse(opt[3])))
                                            {
                                                //if (z - tree.connectionCount(tree.root, 10000000, 99999999) <= 0)
                                                //    ;
                                                //bruh = true;
                                                Graph.delete(int.Parse(opt[2]), int.Parse(opt[3]), opt[1], tree);
                                                roznice++;
                                                //nugget = true;
                                            }
                                            //if (tree.search(opt[1], int.Parse(opt[2]), int.Parse(opt[3]), false) && bruh)
                                            //  ;

                                            //Console.WriteLine(tree.search(opt[1], int.Parse(opt[2]), int.Parse(opt[3]), true));
                                            //tree.write(tree.root);
                                            wszystkie_roznice++;
                                            
                                            //if (nugget!=nugget2)
                                            //{
                                            //    streamWriter.WriteLine(opt[1] + " " + opt[2] + " " + opt[3]);
                                            //    Console.WriteLine("bruh");
                                            //}
                                            //tree.write(tree.root);
                                            break;
                                        }
                                    case "WP"://wyszukaj polaczenie
                                        {
                                            //if (opt[1] == "1400-02-01" && opt[2] == "11" && opt[3] == "14")
                                            //    ;
                                            /*int bruh = amIretarded.IndexOf(new info2(int.Parse(opt[2]), int.Parse(opt[3]), opt[1]));
                                            int bruh2= amIretarded.IndexOf(new info2(int.Parse(opt[3]), int.Parse(opt[2]), opt[1]));
                                            if (bruh > -1 || bruh2>-1)
                                                Console.Write("TAK ");
                                            else Console.Write("NIE ");
                                            */
                                                if (!tree.search(opt[1], int.Parse(opt[2]), int.Parse(opt[3]), opt.Length > 4))
                                                Console.Write("NIE");
                                            else
                                                Console.Write("TAK");
                                            string bruh = plik2.ReadLine();
                                            Console.WriteLine("\t rozw: " +bruh);
                                            break;
                                        }
                                    case "LP"://liczba polaczen pomiedzy
                                        {
                                            //tree.write(tree.root);
                                            //Console.WriteLine(opt[1]+" "+opt[2]);
                                            
                                            int[] tab1 = Array.ConvertAll(opt[1].Split('-'), int.Parse);
                                            int qid1 = tab1[0] * 10000 + tab1[1] * 100 + tab1[2];
                                            int[] tab2 = Array.ConvertAll(opt[2].Split('-'), int.Parse);
                                            int qid2 = tab2[0] * 10000 + tab2[1] * 100 + tab2[2];
                                            //int countbruh = 0;
                                            /*foreach (var x in amIretarded)
                                            {
                                                if (x.qid >= qid1 && x.qid <= qid2)
                                                    countbruh++;

                                            }*/
                                            Console.Write(/*tree.count -countbruh+" LP " + */tree.connectionCount(tree.root, qid1, qid2));
                                            string bruh = plik2.ReadLine();
                                            Console.WriteLine("\t rozw: " + bruh);
                                            break;
                                        }
                                    case "WY"://wypisanie polaczen
                                        {
                                            tree.write(tree.root);
                                            break;
                                        }
                                    case "NP"://droga z miasta do miasta najszybsza
                                        {
                                            int pom = Graph.dijktra_priority(int.Parse(opt[1]), int.Parse(opt[2]));
                                            if(pom==0)
                                                 Console.Write("NIE");
                                            else
                                            {
                                                Console.Write(pom);

                                            }
                                            string bruh = plik2.ReadLine();
                                            Console.WriteLine("\t rozw : " + bruh);
                                            break;
                                        }
                                    case "ND"://droga z miasta do miasta do pewnej daty
                                        {
                                            //break;
                                            //int[] tab = Array.ConvertAll(pom[2].Split('-'), int.Parse);
                                            //int qid = tab[0] * 10000 + tab[1] * 100 + tab[2];
                                            int pom = Graph.dijkstra3(int.Parse(opt[1]), int.Parse(opt[2]), int.Parse(opt[3]));
                                            if (pom == -1)
                                                Console.Write("NIE");
                                            else
                                            {
                                                StringBuilder newString = new StringBuilder();
                                                newString.Append("-" + string.Format(pom % 100<10?"0":"") + pom % 100);
                                                pom /= 100;
                                                newString.Insert(0, "-" + string.Format(pom % 100 < 10 ? "0" : "") + pom % 100);
                                                pom /= 100;
                                                newString.Insert(0, pom);
                                                Console.Write(newString);
                                            }
                                            string bruh = plik2.ReadLine();
                                            Console.WriteLine("\t rozw: " + bruh);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }

                                }
                            }
                            Console.ReadLine();
                            break;
                        }
                    default:
                        {
                            streamWriter.Close();
                            return;

                        }
                }
                            
                
            }
            
        }
        
    }
    
}

