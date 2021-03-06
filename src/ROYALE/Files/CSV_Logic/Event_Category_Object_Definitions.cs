using CRepublic.Royale.Files.CSV_Helpers;
using CRepublic.Royale.Files.CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRepublic.Royale.Files.CSV_Logic
{
    internal class Event_Category_Object_Definitions : Data
    {
        internal Event_Category_Object_Definitions(Row _Row, DataTable _DataTable) : base(_Row, _DataTable)
        {
            Load(_Row);
        }

        // NOTE: This was generated from the event_category_object_definitions.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets Property type.
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// Gets or sets Is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets Object type.
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets Default int.
        /// </summary>
        public int DefaultInt { get; set; }

        /// <summary>
        /// Gets or sets Default string.
        /// </summary>
        public string DefaultString { get; set; }
    }
}