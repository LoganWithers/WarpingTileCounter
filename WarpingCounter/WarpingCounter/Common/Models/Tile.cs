namespace WarpingCounter.Common.Models
{

    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Tile
    {

        public Tile(string name)
        {
            Name = name.Replace("True", "true")
                       .Replace("False", "false");

            ;

            if (name == "seed")
            {
                Color = "red";
                Up    = new Glue("NULL");
            }

            Glues = new List<Glue> {North, South, East, West, Up, Down};
        }


        public Tile() { }


        public string Name { get; private set; }


        protected List<Glue> Glues { get; }


        protected string Label { get; set; } = string.Empty;


        public Glue North { get; set; } = new Glue();


        public Glue South { get; set; } = new Glue();


        public Glue East { get; set; } = new Glue();


        public Glue West { get; set; } = new Glue();


        public Glue Up { get; set; } = new Glue();


        public Glue Down { get; set; } = new Glue();


        public string Color { get; set; }


        public void Prepend(string name)
        {
            if (Name != "seed")
            {
                Name = $"{name} id: {Name}".Replace("True", "true")
                                           .Replace("False", "false");
            }
        }


        /// <summary>
        ///   Attaches a tile north of the tile this is called on.
        ///   <br />
        ///   a.AttachSouth(b) puts b directly north of a.
        /// </summary>
        public void AttachNorth(Tile that)
        {
            var label = Guid.NewGuid()
                            .ToString();

            that.South.Label = label;
            that.South.Bind  = 1;

            North.Bind  = 1;
            North.Label = label;
        }


        /// <summary>
        ///   Attaches a tile west of the tile this is called on.
        ///   <br />
        ///   a.AttachSouth(b) puts b directly west of a.
        /// </summary>
        public void AttachWest(Tile that)
        {
            var label = Guid.NewGuid()
                            .ToString();

            that.East.Label = label;
            that.East.Bind  = 1;

            West.Bind  = 1;
            West.Label = label;
        }


        /// <summary>
        ///   Attaches a tile east of the tile this is called on.
        ///   <br />
        ///   a.AttachSouth(b) puts b directly east of a.
        /// </summary>
        public void AttachEast(Tile that)
        {
            var label = Guid.NewGuid()
                            .ToString();

            that.West.Label = label;
            that.West.Bind  = 1;

            East.Bind  = 1;
            East.Label = label;
        }


        /// <summary>
        ///   Attaches a tile south of the tile this is called on.
        ///   <br />
        ///   a.AttachSouth(b) puts b directly south of a.
        /// </summary>
        public void AttachSouth(Tile that)
        {
            var label = Guid.NewGuid()
                            .ToString();

            that.North.Label = label;
            that.North.Bind  = 1;

            South.Bind  = 1;
            South.Label = label;
        }


        /// <summary>
        ///   Attaches a tile above the tile this is called on.
        ///   <br />
        ///   a.AttachAbove(b) puts b directly on top of a in Z=1.
        /// </summary>
        public void AttachAbove(Tile that)
        {
            var label = Guid.NewGuid()
                            .ToString();

            that.Down.Label = label;
            that.Down.Bind  = 1;

            Up.Bind  = 1;
            Up.Label = label;
        }


        /// <summary>
        ///   Attaches a tile below the tile this is called on.
        ///   <br />
        ///   a.AttachBelow(b) puts b directly on below of a in Z=0.
        /// </summary>
        public void AttachBelow(Tile that)
        {
            var label = Guid.NewGuid()
                            .ToString();

            that.Up.Label = label;
            that.Up.Bind  = 1;

            Down.Bind  = 1;
            Down.Label = label;
        }


        //public override string ToString()
        //{
        //    var sb = new StringBuilder()
        //            .Append($"TILENAME {Name}")
        //            .AppendLine()
        //            .Append($"NORTHBIND {North.Bind}")
        //            .AppendLine()
        //            .Append($"EASTBIND {East.Bind}")
        //            .AppendLine()
        //            .Append($"SOUTHBIND {South.Bind}")
        //            .AppendLine()
        //            .Append($"WESTBIND {West.Bind}")
        //            .AppendLine()
        //            .Append($"UPBIND {Up.Bind}")
        //            .AppendLine()
        //            .Append($"DOWNBIND {Down.Bind}")
        //            .AppendLine()
        //            .Append($"NORTHLABEL {North.Label}")
        //            .AppendLine()
        //            .Append($"EASTLABEL {East.Label}")
        //            .AppendLine()
        //            .Append($"SOUTHLABEL {South.Label}")
        //            .AppendLine()
        //            .Append($"WESTLABEL {West.Label}")
        //            .AppendLine()
        //            .Append($"UPLABEL {Up.Label}")
        //            .AppendLine()
        //            .Append($"DOWNLABEL {Down.Label}")
        //            .AppendLine();


        //    if (!string.IsNullOrEmpty(Color))
        //    {
        //        sb.Append($"TILECOLOR {Color}")
        //          .AppendLine();
        //    }

        //    return sb.Append("CREATE")
        //             .AppendLine()
        //             .ToString();
        //}


        public override string ToString()
        {
            var sb = new StringBuilder()
           .AppendLine($"TILENAME {Name}");

            if (North.Bind > 0)
            {
                sb.AppendLine($"NORTHBIND {North.Bind}")
                  .AppendLine($"NORTHLABEL {North.Label}");
            }

            if (East.Bind > 0)
            {
                sb.AppendLine($"EASTBIND {East.Bind}")
                  .AppendLine($"EASTLABEL {East.Label}");
            }

            if (South.Bind > 0)
            {
                sb.AppendLine($"SOUTHBIND {South.Bind}")
                  .AppendLine($"SOUTHLABEL {South.Label}");
            }

            if (West.Bind > 0)
            {
                sb.AppendLine($"WESTBIND {West.Bind}")
                  .AppendLine($"WESTLABEL {West.Label}");
            }

            if (Up.Bind > 0)
            {
                sb.AppendLine($"UPBIND {Up.Bind}")
                  .AppendLine($"UPLABEL {Up.Label}");
            }

            if (Down.Bind > 0)
            {
                sb.AppendLine($"DOWNBIND {Down.Bind}")
                  .AppendLine($"DOWNLABEL {Down.Label}");
            }

            if (!string.IsNullOrEmpty(Color))
            {
                sb.AppendLine($"TILECOLOR {Color}");
            }

            return sb.AppendLine("CREATE")
                     .ToString();
        }

    }

}
