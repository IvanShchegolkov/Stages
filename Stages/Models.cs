using System;
using System.Collections.Generic;
using System.Text;

namespace Stages
{
    internal class Stages
    {
        public string Name { get; set; }
        public string Performer { get; set; }
    }
    internal class StagesList
    {
        public List<Stages> Stages { get; set; }
    }
    internal class MoveStages
    {
        public string Name { get; set; }
        public string Performer { get; set; }
        public string Decision { get; set; }
        public string Comment { get; set; }
    }
}