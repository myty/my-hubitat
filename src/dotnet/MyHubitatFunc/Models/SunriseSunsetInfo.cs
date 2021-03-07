namespace MyHubitatFunc.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class SunriseSunsetInfo
    {
        [JsonProperty("results")]
        public Results Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Results
    {
        [JsonProperty("sunrise")]
        public DateTimeOffset Sunrise { get; set; }

        [JsonProperty("sunset")]
        public DateTimeOffset Sunset { get; set; }

        [JsonProperty("solar_noon")]
        public DateTimeOffset SolarNoon { get; set; }

        [JsonProperty("day_length")]
        public long DayLength { get; set; }

        [JsonProperty("civil_twilight_begin")]
        public DateTimeOffset CivilTwilightBegin { get; set; }

        [JsonProperty("civil_twilight_end")]
        public DateTimeOffset CivilTwilightEnd { get; set; }

        [JsonProperty("nautical_twilight_begin")]
        public DateTimeOffset NauticalTwilightBegin { get; set; }

        [JsonProperty("nautical_twilight_end")]
        public DateTimeOffset NauticalTwilightEnd { get; set; }

        [JsonProperty("astronomical_twilight_begin")]
        public DateTimeOffset AstronomicalTwilightBegin { get; set; }

        [JsonProperty("astronomical_twilight_end")]
        public DateTimeOffset AstronomicalTwilightEnd { get; set; }
    }

    public partial class SunriseSunsetInfo
    {
        public static SunriseSunsetInfo FromJson(string json) => JsonConvert.DeserializeObject<SunriseSunsetInfo>(json, SunriseSunsetInfoConverter.Settings);
    }

    public static class SerializeSunriseSunsetInfo
    {
        public static string ToJson(this SunriseSunsetInfo self) => JsonConvert.SerializeObject(self, SunriseSunsetInfoConverter.Settings);
    }

    internal static class SunriseSunsetInfoConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
