using System;
using System.Collections.Generic;
using System.Linq;

namespace upr4
{
    public class Contact
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value ?? $"user{new Random().Next(1000, 9999)}";
        }

        public Contact(string name) => Name = name;
    }

    public class Message
    {
        public Contact Author { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public bool IsEdited { get; set; } = false;

        public Message(Contact author, string text)
        {
            Author = author;
            Text = text;
            Created = DateTime.Now;
        }

        public void Deconstruct(out string author, out string text)
        {
            author = Author.Name;
            text = Text;
        }

        public void Deconstruct(out DateTime date, out string time, out bool isEdited)
        {
            date = Created.Date;
            time = Created.ToShortTimeString();
            isEdited = IsEdited;
        }
    }

    public class ChatRoom
    {
        public string Name { get; set; }
        public List<Contact> Users { get; set; }
        public List<Message> Messages { get; set; }

        public ChatRoom(string name)
        {
            Name = name;
            Users ??= new List<Contact>();
            Messages ??= new List<Message>();
        }

        public void AddEntity(object entity)
        {
            switch (entity)
            {
                case Contact c when c.Name.StartsWith("Admin"):
                    Console.WriteLine($"[System] Admin {c.Name} joined.");
                    Users.Add(c);
                    break;
                case Contact c:
                    Console.WriteLine($"User {c.Name} joined.");
                    Users.Add(c);
                    break;
                case Message m:
                    Messages.Add(m);
                    Console.WriteLine("Message sent.");
                    break;
                default:
                    Console.WriteLine("Unknown entity.");
                    break;
            }
        }

        public (string TopUser, int Count, string ShortestMsg) GetStats()
        {
            if (!Messages.Any()) return ("None", 0, "None");

            var top = Messages
                .GroupBy(m => m.Author)
                .OrderByDescending(g => g.Count())
                .First();

            string shortest = top.OrderBy(m => m.Text.Length).First().Text;

            return (top.Key.Name, top.Count(), shortest);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ChatRoom room = new ChatRoom("General");
            Contact me = new Contact(null);
            Console.WriteLine($"Welcome, {me.Name}");

            while (true)
            {
                Console.WriteLine("\n1. Add Contact\n2. Send Message\n3. Stats\n4. Deconstruct Last\n5. View Last (Index)\n0. Exit");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Name: ");
                        string n = Console.ReadLine();
                        room.AddEntity(new Contact(string.IsNullOrWhiteSpace(n) ? null : n));
                        break;
                    case "2":
                        Console.Write("Msg: ");
                        string t = Console.ReadLine();
                        room.AddEntity(new Message(me, t));
                        break;
                    case "3":
                        var stats = room.GetStats();
                        Console.WriteLine($"Top: {stats.TopUser} ({stats.Count}), Shortest: {stats.ShortestMsg}");
                        break;
                    case "4":
                        if (room.Messages.Any())
                        {
                            var lastMsg = room.Messages.Last();

                            var (author, content) = lastMsg;
                            Console.WriteLine($"Deconstructed Info: {author} said '{content}'");

                            var (date, time, edited) = lastMsg;
                            Console.WriteLine($"Time: {time} on {date.ToShortDateString()}. Edited: {edited}");
                        }
                        else Console.WriteLine("No messages to deconstruct.");
                        break;
                    case "5":
                        if (room.Messages.Count > 0)
                        {
                            Console.WriteLine($"Last: {room.Messages[^1].Text}");
                        }
                        break;
                    case "0": return;
                }
            }
        }
    }
}