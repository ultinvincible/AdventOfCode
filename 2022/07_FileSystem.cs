using System;
using System.Collections.Generic;

namespace Advent_of_Code._2022
{
    class _07_FileSystem : AoCDay
    {
        const int RequiredUsedSpace = 40000000;
        // 70000000 - 30000000
        class Directory
        {
            public string name;
            public Directory parent;
            public List<Directory> children;
            public int size;
            public bool isFile;

            public Directory(string n, Directory p,
                int s = 0, bool file = false)
            {
                name = n;
                parent = p;
                children = new();
                size = s;
                isFile = file;
            }
            public void AddChild(string name, int size = 0, bool isFile = false)
                => children.Add(new(name, this, size, isFile));
        }
        protected override void Run()
        {
            //debug = true;
            Directory root = new("/", null), current = root;
            foreach (string line in inputLines[1..])
            {
                if (line[..4] == "$ cd")
                {
                    if (line[5..] == "..")
                        current = current.parent;
                    else current = current.children.
                            Find(d => d.name == line[5..]);
                    if (current is null) throw new Exception("Missing child");
                }
                else if (line[..4] == "dir ")
                    current.AddChild(line[4..]);
                else if (line != "$ ls")
                {
                    string[] split = line.Split(' ');
                    current.AddChild(split[1], int.Parse(split[0]), true);
                }
            }

            GetSize(root);

            part2 = int.MaxValue;
            minDelete = root.size - RequiredUsedSpace;
            CheckDelete(root);

            if (debug)
            {
                string print = "";
                PrintFileTree(root, ref print);
                Console.WriteLine(print);
            }
        }
        void GetSize(Directory dir)
        {
            foreach (Directory child in dir.children)
            {
                if (!dir.isFile)
                    GetSize(child);
                dir.size += child.size;
            }

            if (!dir.isFile && dir.size <= 100000) part1 += dir.size;
        }

        static int minDelete;
        void CheckDelete(Directory dir)
        {
            if (!dir.isFile && dir.size >= minDelete)
                part2 = Math.Min(dir.size, part2);
            foreach (Directory child in dir.children)
                CheckDelete(child);
        }

        void PrintFileTree(Directory dir,
            ref string print, int depth = 0)
        {
            print += $"{new string(' ', depth * 2)}{depth}/{dir.name}" +
                $": {dir.size}{(!dir.isFile && dir.size <= 100000 ? " #" : "")}\n";
            foreach (Directory child in dir.children)
                PrintFileTree(child, ref print, depth + 1);
        }
    }
}
