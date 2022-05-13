using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IWork
{
    class MyDictionary:Dictionary<IPart,int>
    {
        public bool ContainsIPart(IPart ip)
        {
            foreach (var item in this.Keys)
            {
                if (item.name()==ip.name())
                    return true;
            }
            return false;
        }
        public int IPartCount(IPart ip)
        {
            int count = 0;
            foreach (var item in this.Keys)
            {
                if (item.name() == ip.name())
                    count=this[item];
            }
            return count;
        }
        public void PlusKey(IPart ip)
        {
            foreach (var item in this.Keys)
            {
                if (item.name() == ip.name())
                    this[item]++;
            }
        }
    }
    interface IWorker
    {
        void Work(IPart ip);
    }

    interface IPart
    {
        string name();
    }

    class Basement : IPart
    {
        public string name() { return "фундамент"; }
        
    }
    class Walls : IPart
    {
        public string name() { return "стіни"; }
    }

    class Door : IPart
    {
        public string name() { return "двері"; }
    }

    class Window : IPart
    {
        public string name() { return "вікно"; }
    }

    class Roof : IPart
    {
        public string name() { return "дах"; }
    }

    class Worker:IWorker
    {
        public void Work(IPart ip)
        {
            Console.WriteLine("робітник будує "+ip.name());
        }
    }

    class TeamLeader:IWorker
    {        
        public void Work(IPart ip)
        {
            Console.WriteLine("прораб командує будівництвом "+ip.name());
        }
        public void Print(ref House h)
        {
            Console.WriteLine("На даний момент побудовано:");
            foreach (var item in h.GetParts())
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }
        }
        public IPart SetTask(ref House h)
        {
            if (!h.GetParts().ContainsIPart(new Basement()))
                return new Basement();
            else if (h.GetParts().ContainsIPart(new Basement()) && (!h.GetParts().ContainsIPart(new Walls()) || h.GetParts().IPartCount(new Walls()) < 4))
                return new Walls();
            else if (h.GetParts().ContainsIPart(new Basement()) && h.GetParts().IPartCount(new Walls()) == 4 && !h.GetParts().ContainsIPart(new Door()))
                return new Door();
            else if (h.GetParts().ContainsIPart(new Basement()) && h.GetParts().IPartCount(new Walls()) == 4 && h.GetParts().ContainsIPart(new Door()) && (!h.GetParts().ContainsIPart(new Window()) || h.GetParts().IPartCount(new Window()) < 4))
                return new Window();
            else if (h.GetParts().ContainsIPart(new Basement()) && h.GetParts().IPartCount(new Walls()) == 4 && h.GetParts().ContainsIPart(new Door()) && h.GetParts().IPartCount(new Window()) == 4&&!h.GetParts().ContainsIPart(new Roof()))
                return new Roof();
            else return null;
        }
    }
    
    class House
    {
        MyDictionary parts=new MyDictionary();
        public MyDictionary GetParts() { return parts; }
        public void addPart(IPart p)
        {
            if (p is Basement==true && parts.ContainsIPart(p)==true)
                throw new Exception("Фундамент вже побудований");
            else if (p is Window==true && parts.ContainsIPart(p)==true && parts.IPartCount(p) == 4)
                throw new Exception("4 вікна вже збудовано");
            else if (p is Door==true && parts.ContainsIPart(p)==true)
                throw new Exception("Двері вже збудовано");
            else if (p is Walls==true && parts.ContainsIPart(p)==true && parts.IPartCount(p) == 4)
                throw new Exception("4 cтіни вже збудовано");
            else if (p is Roof==true && parts.ContainsIPart(p)==true)
                throw new Exception("Дах вже збудовано");
            else if (parts.ContainsIPart(p)==false)
                parts.Add(p, 1);
            else if(parts.ContainsIPart(p)==true&&(p is Window==true||p is Walls==true)&& parts.IPartCount(p) < 4)
                parts.PlusKey(p);
            else Console.WriteLine("Дім збудовано");
        }
    }

    class Team
    {
        IWorker[] team =null;
        public Team(int count) 
        { 
            team = new IWorker[count];
            for (int i = 0; i < count; i++)
            {
                if(i==0)
                    team[i] = new TeamLeader();
                else
                    team[i] = new Worker();
            }
        }
        public void Build(ref House h)
        {
            IPart obj=null;            
            foreach (var item in team)
            {
                if (item is TeamLeader)
                {
                    obj = (item as TeamLeader).SetTask(ref h);
                    if (obj != null)
                        (item as TeamLeader).Work(obj);
                    else
                    {
                        Console.WriteLine("Будинок збудовано");
                        return;
                    }
                }
                
            }
            foreach (var item in team)
            {
                if (item is Worker)
                {
                    (item as Worker).Work(obj);
                }
            }
            try
            {
                h.addPart(obj);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                
                Console.Read();
            }
            Console.WriteLine();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            House h=new House();
            Team team=new Team(4);
            while (true)
            {
                team.Build(ref h);
                Console.Read();
            }
        }
    }
}
