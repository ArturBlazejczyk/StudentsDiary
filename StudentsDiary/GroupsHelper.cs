using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudentsDiary
{
    public class GroupsHelper
    {
        public static List<Group> GetGroups(string defualtName)
        {
            return new List<Group>()
            {
                new Group() { Id = 0, Name = defualtName},
                new Group() { Id = 1, Name = "Grupa 1"},
                new Group() { Id = 2, Name = "Grupa 2"},
                new Group() { Id = 3, Name = "Grupa 3"},
            };
        }

    }
}
