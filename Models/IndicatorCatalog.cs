using System.ComponentModel.DataAnnotations;

namespace ApiDocBot.Models
{
    public class IndicatorCatalog
    {
        [Key]
        public int indicatorcatalog_id { get; set; }
        public string indicatorcatalog_name { get; set; }=string.Empty;
        public string indicatorcatalog_description { get; set; } = string.Empty;
        public int indicatorcatalog_scaleMin { get; set; }
        public int indicatorcatalog_scaleMax { get; set; }

        public IndicatorCatalog()
        {

        }

        public IndicatorCatalog(int indicatorcatalog_id, string indicatorcatalog_name, string indicatorcatalog_description, int indicatorcatalog_scaleMin, int indicatorcatalog_scaleMax)
        {
            this.indicatorcatalog_id = indicatorcatalog_id;
            this.indicatorcatalog_name = indicatorcatalog_name;
            this.indicatorcatalog_description = indicatorcatalog_description;
            this.indicatorcatalog_scaleMin = indicatorcatalog_scaleMin;
            this.indicatorcatalog_scaleMax = indicatorcatalog_scaleMax;
        }
    }
}
