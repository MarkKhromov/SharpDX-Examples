using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDXExamples {
    public static class Menu {
        class Item {
            public Item(int id, string header, Action action) {
                Id = id;
                Header = header;
                this.action = action;
            }

            public int Id { get; }
            public string Header { get; }
            readonly Action action;

            public void Invoke() {
                action();
            }
        }

        static Menu() {
            canExit = false;
            items = new List<Item>();
            RegisterItem("Exit", () => canExit = true);
        }

        static readonly IList<Item> items;
        static bool canExit;

        public static void RegisterItem(string header, Action action) {
            items.Add(new Item(items.Count, header, action));
        }

        public static void Show() {
            while(!canExit) {
                Console.Clear();
                Console.WriteLine('\t');
                Console.WriteLine("\tMenu:");
                if(items.Any())
                    Console.WriteLine();
                for(int i = 1; i < items.Count; i++) {
                    Console.WriteLine($"\t{items[i].Id}. {items[i].Header}");
                }
                Console.WriteLine();
                Console.WriteLine($"\t{items[0].Id}. {items[0].Header}");
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("\tYour choose: ");
                int selectedItem;
                if(int.TryParse(Console.ReadLine(), out selectedItem)) {
                    items[selectedItem].Invoke();
                }
            }
        }
    }
}