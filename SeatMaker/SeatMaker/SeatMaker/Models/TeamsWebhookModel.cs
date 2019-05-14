using System;
using System.Collections.Generic;
using System.Text;

namespace SeatMaker.Models
{
    internal class TeamsWebhookModel
    {
        public string Type { get; set; }
        public string Context { get; set; }
        public string Summary { get; set; }
        public string ThemeColor { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<Section> Sections { get; set; }
        public List<Potentialaction> PotentialAction { get; set; }
    }
    internal class Section
    {
        public string ActivityTitle { get; set; }
        public string ActivitySubtitle { get; set; }
        public List<Fact> Facts { get; set; }
        public string Text { get; set; }
        public Boolean Markdown { get; set; }
    }
    internal class Target
    {
        public string Os { get; set; }
        public string Uri { get; set; }
    }
    internal class Potentialaction
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<Target> Targets { get; set; }
    }
    internal class Fact
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
