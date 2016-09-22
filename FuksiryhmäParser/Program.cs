using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FuksiryhmäParser
{
    class Program
    {
        static void Main(string[] args)
        {
            // parses a text file and serializes the contents to json for use in kroniikkamaatti's state.js
            // guild, tutors and student identification is based on indentation
            // first "level" is guild, second is tutors and third is student
            // see tuutoriryhmät.txt

            string jsstart = @"import { fromJS } from 'immutable';

const initialState = {
'active':'',
'guilds': ";
            var groupfile = File.ReadAllLines("tuutoriryhmät.txt");
            List<Guild> guilds = new List<Guild>();
            Guild guild = null;
            Group group = null;
            for (int i = 0; i < groupfile.Length; i++)
            {
                if (groupfile[i].StartsWith("\t\t"))
                {
                    // student
                    if (guild == null)
                    {
                        Console.WriteLine("Guild was null when handling student on line " + i + 1);
                        return;
                    }

                    if (group == null)
                    {
                        Console.WriteLine("Group was null when handling student on line " + i + 1);
                        return;
                    }

                    group.students.Add(new Name(groupfile[i].Trim()));
                }

                else if (groupfile[i].StartsWith("\t"))
                {
                    // group
                    if (guild == null)
                    {
                        Console.WriteLine("Guild was null when handling group on line " + i + 1);
                        return;
                    }

                    if (group != null)
                    {
                        guild.groups.Add(group);
                    }

                    group = new Group();

                    // add tutors
                    foreach (string s in groupfile[i].Split(new string[] { " ja " }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        group.tutors.Add(s.Trim());
                    }
                }

                else
                {
                    if (guild != null)
                    {
                        guilds.Add(guild);
                    }
                    // guild
                    guild = new Guild(groupfile[i].Trim());
                }
            }
            guilds.Add(guild);

            var fs = File.Create("state.js");
            var json = JsonConvert.SerializeObject(guilds, Formatting.Indented);
            var jsend = @"

export const immutableState = fromJS(initialState);";
            fs.Close();

            File.WriteAllText("state.js", jsstart + json + jsend);
        }
    }

    class Guild
    {
        public string guild;
        public List<Group> groups;

        public Guild(string guild)
        {
            this.guild = guild;
            this.groups = new List<Group>();
        }
    }

    class Group
    {
        public List<string> tutors;
        public List<Name> students;

        public Group()
        {
            this.tutors = new List<string>();
            this.students = new List<Name>();
        }
    }

    class Name
    {
        public string name;

        public Name(string name)
        {
            this.name = name;
        }
    }
}