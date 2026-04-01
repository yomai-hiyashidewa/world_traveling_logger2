using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Enumeration
{
    //ISO 3166-1 
    public enum CountryType
    {
        [Display(Name = "Japan")]
        JPN,    // 	Japan
        [Display(Name = "No Country")]
        UNK,    // unknown
        // East Asia
        [Display(Name = "South Korea")]
        KOR,    // Korea (the Republic of)
        [Display(Name = "Taiwan")]
        TWN,    // TTaiwan, Province of China
        [Display(Name = "Hong Kong")]
        HKG,    // Hong Kong
        [Display(Name = "Philippines")]
        PHL,    // Philippines
        [Display(Name = "Viet Nam")]
        VNM,    // Viet Nam
        [Display(Name = "Malaysia")]
        MYS,    // Malaysia

        // oceania
        [Display(Name = "Australia")]
        AUS,    // Australia
        
        // north america
        [Display(Name = "U.S.A")]
        USA,    // United States of America
        [Display(Name = "Canada")]
        CAN,    // Canada

        // central america
        [Display(Name = "Mexico")]
        MEX,    // Mexico
        [Display(Name = "Guatemala")]
        GTM,    // Guatemala
        [Display(Name = "Belize")]
        BLZ,    // Belize
        [Display(Name = "El Salvador")]
        SLV,    // El Salvador
        [Display(Name = "Honduras")]
        HND,    // Honduras
        [Display(Name = "Nicaragua")]
        NIC,    // Nicaragua
        [Display(Name = "Costa Rica")]
        CRI,    // Costa Rica
        [Display(Name = "Panama")]
        PAN,    // Panama
        [Display(Name = "Cuba")]
        CUB,    // Cuba
        // south america
        [Display(Name = "Colombia")]
        COL,    // Colombia
        [Display(Name = "Ecuador")]
        ECU,    // Ecuador
        [Display(Name = "Pérou")]
        PER,    // Pérou
        [Display(Name = "Bolivia")]
        BOL,    // Bolivia (Plurinational State of)
        [Display(Name = "Chile")]
        CHL,    // Chile
        [Display(Name = "Paraguay")]
        PRY,    // Paraguay
        [Display(Name = "Argentina")]
        ARG,    // Argentina
        [Display(Name = "Uruguay")]
        URY,    // Uruguay
        [Display(Name = "Brazil")]
        BRA,    // Brazil

        
        // Europe
        [Display(Name = "Iceland")]
        ISL,    // Iceland
        // UK
        [Display(Name = "Ireland")]
        IRL,    // Ireland
        [Display(Name = "UK")]
        GBR,    // United Kingdom of Great Britain and Northern Ireland
        // north Europe
        [Display(Name = "Norway")]
        NOR,    // Norway
        [Display(Name = "Sweden")]
        SWE,    // Sweden
        [Display(Name = "Finland")]
        FIN,    // Finland
        [Display(Name = "Denmark")]
        DNK,    // Denmark
                // west Europe
        [Display(Name = "Portugal")]
        PRT,    // Portugal
        [Display(Name = "Spain")]
        ESP,    // Spain
        [Display(Name = "Andorra")]
        AND,    // Andorra
        [Display(Name = "France")]
        FRA,    // France
        [Display(Name = "Monaco")]
        MCO,    // Monaco
        [Display(Name = "Belgium")]
        BEL,    // Belgium
        [Display(Name = "Luxembourg")]
        LUX,    // Luxembourg
        [Display(Name = "Netherlands")]
        NLD,    // Netherlands
        [Display(Name = "Germany")]
        DEU,    // Germany
        [Display(Name = "Switzerland")]
        CHE,    // Switzerland
        [Display(Name = "Liechtenstein")]
        LIE,    // Liechtenstein
        [Display(Name = "Austria")]
        AUT,    // Austria
        [Display(Name = "Hungary")]
        HUN,    // Hungary
        [Display(Name = "Czechia")]
        CZE,    // Czechia
        [Display(Name = "Slovakia")]
        SVK,    // Slovakia
        [Display(Name = "Poland")]
        POL,    // Poland
        [Display(Name = "Italy")]
        ITA,    // Italy
        [Display(Name = "Knights of Malta")]
        MOM,    // Malta Knight
        [Display(Name = "Vatican City")]
        VAT,    // Holy See
        [Display(Name = "San Marino")]
        SMR,    // San Marino
        [Display(Name = "Estonia")]
        EST,    // Estonia
        [Display(Name = "Latvia")]
        LVA,    // Latvia
        [Display(Name = "Lithuania")]
        LTU,    // Lithuania
        [Display(Name = "Croatia")]
        HRV,    // Croatia
        [Display(Name = "Slovenia")]
        SVN,    // Slovenia
        [Display(Name = "B&H")]
        BIH,    // Bosnia and Herzegovina
        [Display(Name = "Serbia")]
        SRB,    // Serbia
        [Display(Name = "Kosovo")]
        KVX,    // Kosovo
        [Display(Name = "Montenegro")]
        MNE,    // Montenegro
        [Display(Name = "North Macedonia")]
        MKD,    // North Macedonia
        [Display(Name = "Bulgaria")]
        BGR,    // Bulgaria
        [Display(Name = "Romania")]
        ROU,    // Romania
        [Display(Name = "Moldova")]
        MDA,    // Moldova, Republic of
        [Display(Name = "Albania")]
        ALB,    // Albania
        [Display(Name = "Greece")]
        GRC,    // Greece
       
        [Display(Name = "Cyprus")]
        CYP,    // Cyprus
        [Display(Name = "Malta")]
        MLT,    // Malta
        // north africa
        [Display(Name = "Egypt")]
        EGY,    // Egypt
        [Display(Name = "Tunisia")]
        TUN,    // Tunisia
        [Display(Name = "Morocco")]
        MAR,    // Morocco
                // Central Asia
        [Display(Name = "Turkiye")]
        TUR,    // Turkiye
        [Display(Name = "Qatar")]
        QAT,    // Qatar
        [Display(Name = "UAE")]
        ARE,    // United Arab Emirates
        [Display(Name = "Georgia")]
        GEO,    // Georgia
        [Display(Name = "Armenia")]
        ARM,    // Armenia
        [Display(Name = "Uzbekistan")]
        UZB,    // Uzbekistan
        // South Asia
        [Display(Name = "India")]
        IND,    // India
        [Display(Name = "North Cyprus")]
        NCY,    // Cyprus
                // not yet
        [Display(Name = "Azerbaijan")]
        AZE,    // Azerbaijan
        [Display(Name = "Afghanistan")]
        AFG,    // Afghanistan
        [Display(Name = "Algeria")]
        DZA,    // Algeria
        [Display(Name = "Aruba")]
        ABW,    // Aruba
        [Display(Name = "Anguilla")]
        AIA,    // Anguilla
        [Display(Name = "Angola")]
        AGO,    // Angola
        [Display(Name = "Antigua and Barbuda")]
        ATG,    // Antigua and Barbuda
        [Display(Name = "Yemen")]
        YEM,    // Yemen
        [Display(Name = "Israel")]
        ISR,    // Israel
        [Display(Name = "Iraq")]
        IRQ,    // Iraq
        [Display(Name = "Iran")]
        IRN,    // Iran (Islamic Republic of)
        [Display(Name = "Indonesia")]
        IDN,    // Indonesia
        [Display(Name = "Wallis and Futuna")]
        WLF,    // Wallis and Futuna
        [Display(Name = "Uganda")]
        UGA,    // Uganda
        [Display(Name = "Ukraine")]
        UKR,    // Ukraine
        [Display(Name = "Eswatini")]
        SWZ,    // Eswatini
        [Display(Name = "Ethiopia")]
        ETH,    // Ethiopia
        [Display(Name = "Eritrea")]
        ERI,    // Eritrea
        [Display(Name = "Oman")]
        OMN,    // Oman
        [Display(Name = "Ghana")]
        GHA,    // Ghana
        [Display(Name = "Cabo Verde")]
        CPV,    // Cabo Verde
        [Display(Name = "Guyana")]
        GUY,    // Guyana
        [Display(Name = "Kazakhstan")]
        KAZ,    // Kazakhstan
        [Display(Name = "Gabon")]
        GAB,    // Gabon
        [Display(Name = "Cameroon")]
        CMR,    // Cameroon
        [Display(Name = "Gambia")]
        GMB,    // Gambia
        [Display(Name = "Cambodia")]
        KHM,    // Cambodia
        [Display(Name = "Guinea")]
        GIN,    // Guinea
        [Display(Name = "Guinea-Bissau")]
        GNB,    // Guinea-Bissau
        [Display(Name = "Curaçao")]
        CUW,    // Curaçao
        [Display(Name = "Kiribati")]
        KIR,    // Kiribati
        [Display(Name = "Kyrgyzstan")]
        KGZ,    // Kyrgyzstan
        [Display(Name = "Kuwait")]
        KWT,    // Kuwait
        [Display(Name = "Grenada")]
        GRD,    // Grenada
        [Display(Name = "Kenya")]
        KEN,    // Kenya
        [Display(Name = "Côte d'Ivoire")]
        CIV,    // Côte d'Ivoire
        [Display(Name = "Comoros")]
        COM,    // Comoros
        [Display(Name = "Congo")]
        COG,    // Congo
        [Display(Name = "Congo, Democratic Republic of the")]
        COD,    // Congo, Democratic Republic of the
        [Display(Name = "Saudi Arabia")]
        SAU,    // Saudi Arabia
        [Display(Name = "Samoa")]
        WSM,    // Samoa
        [Display(Name = "Zambia")]
        ZMB,    // Zambia
        [Display(Name = "Sierra Leone")]
        SLE,    // Sierra Leone
        [Display(Name = "Djibouti")]
        DJI,    // Djibouti
        [Display(Name = "Jamaica")]
        JAM,    // Jamaica
        [Display(Name = "Syria")]
        SYR,    // Syrian Arab Republic
        [Display(Name = "Singapore")]
        SGP,    // Singapore
        [Display(Name = "Zimbabwe")]
        ZWE,    // Zimbabwe
        [Display(Name = "Sudan")]
        SDN,    // Sudan
        [Display(Name = "Suriname")]
        SUR,    // Suriname
        [Display(Name = "Sri Lanka")]
        LKA,    // Sri Lanka
        [Display(Name = "Seychelles")]
        SYC,    // Seychelles
        [Display(Name = "Senegal")]
        SEN,    // Senegal
        [Display(Name = "Saint Lucia")]
        LCA,    // Saint Lucia
        [Display(Name = "Somalia")]
        SOM,    // Somalia
        [Display(Name = "Thailand")]
        THA,    // Thailand
        [Display(Name = "Tajikistan")]
        TJK,    // Tajikistan
        [Display(Name = "Tanzania")]
        TZA,    // Tanzania, United Republic of
        [Display(Name = "Chad")]
        TCD,    // Chad
        [Display(Name = "Central African Republic")]
        CAF,    // Central African Republic
        [Display(Name = "China")]
        CHN,    // China
        [Display(Name = "North Korea")]
        PRK,    // Korea (the Democratic People's Republic of)
        [Display(Name = "Tuvalu")]
        TUV,    // Tuvalu
        [Display(Name = "Togo")]
        TGO,    // Togo
        [Display(Name = "Tokelau")]
        TKL,    // Tokelau
        [Display(Name = "Dominican Republic")]
        DOM,    // Dominican Republic
        [Display(Name = "Dominica")]
        DMA,    // Dominica
        [Display(Name = "Trinidad and Tobago")]
        TTO,    // Trinidad and Tobago
        [Display(Name = "Turkmenistan")]
        TKM,    // Turkmenistan
        [Display(Name = "Tonga")]
        TON,    // Tonga
        [Display(Name = "Nigeria")]
        NGA,    // Nigeria
        [Display(Name = "Nauru")]
        NRU,    // Nauru
        [Display(Name = "Namibia")]
        NAM,    // Namibia
        [Display(Name = "Antarctica")]
        ATA,    // Antarctica
        [Display(Name = "Niue")]
        NIU,    // Niue
        [Display(Name = "Niger")]
        NER,    // Niger
        [Display(Name = "Western Sahara")]
        ESH,    // Western Sahara
        [Display(Name = "New Caledonia")]
        NCL,    // New Caledonia
        [Display(Name = "New Zealand")]
        NZL,    // New Zealand
        [Display(Name = "Nepal")]
        NPL,    // Nepal
        [Display(Name = "Bahrain")]
        BHR,    // Bahrain
        [Display(Name = "Haiti")]
        HTI,    // Haiti
        [Display(Name = "Pakistan")]
        PAK,    // Pakistan
        [Display(Name = "Vanuatu")]
        VUT,    // Vanuatu
        [Display(Name = "Bahamas")]
        BHS,    // Bahamas
        [Display(Name = "Papua New Guinea")]
        PNG,    // Papua New Guinea
        [Display(Name = "Bermuda")]
        BMU,    // Bermuda
        [Display(Name = "Palau")]
        PLW,    // Palau
        [Display(Name = "Barbados")]
        BRB,    // Barbados
        [Display(Name = "Palestine")]
        PSE,    // Palestine, State of
        [Display(Name = "Bangladesh")]
        BGD,    // Bangladesh
        [Display(Name = "Timor-Leste")]
        TLS,    // Timor-Leste
        [Display(Name = "Pitcairn")]
        PCN,    // Pitcairn
        [Display(Name = "Fiji")]
        FJI,    // Fiji
        [Display(Name = "Bhutan")]
        BTN,    // Bhutan
        [Display(Name = "Puerto Rico")]
        PRI,    // Puerto Rico
        [Display(Name = "Faroe Islands")]
        FRO,    // Faroe Islands
        [Display(Name = "Burkina Faso")]
        BFA,    // Burkina Faso
        [Display(Name = "Brunei")]
        BRN,    // Brunei Darussalam
        [Display(Name = "Burundi")]
        BDI,    // Burundi
        [Display(Name = "Benin")]
        BEN,    // Benin
        [Display(Name = "Venezuela")]
        VEN,    // Venezuela (Bolivarian Republic of)
        [Display(Name = "Belarus")]
        BLR,    // Belarus
        [Display(Name = "Botswana")]
        BWA,    // Botswana
        [Display(Name = "Macau")]
        MAC,    // Macau
        [Display(Name = "Madagascar")]
        MDG,    // Madagascar
        [Display(Name = "Mayotte")]
        MYT,    // Mayotte
        [Display(Name = "Malawi")]
        MWI,    // Malawi
        [Display(Name = "Mali")]
        MLI,    // Mali
        [Display(Name = "Micronesia")]
        FSM,    // Micronesia (Federated States of)
        [Display(Name = "South Africa")]
        ZAF,    // South Africa
        [Display(Name = "South Sudan")]
        SSD,    // South Sudan
        [Display(Name = "Myanmar")]
        MMR,    // Myanmar
        [Display(Name = "Mauritius")]
        MUS,    // Mauritius
        [Display(Name = "Mauritania")]
        MRT,    // Mauritania
        [Display(Name = "Mozambique")]
        MOZ,    // Mozambique
        [Display(Name = "Maldives")]
        MDV,    // Maldives
        [Display(Name = "Mongolia")]
        MNG,    // Mongolia
        [Display(Name = "Jordan")]
        JOR,    // Jordan
        [Display(Name = "Laos")]
        LAO,    // Lao People's Democratic Republic
        [Display(Name = "Libya")]
        LBY,    // Libya
        [Display(Name = "Liberia")]
        LBR,    // Liberia
        [Display(Name = "Rwanda")]
        RWA,    // Rwanda
        [Display(Name = "Lesotho")]
        LSO,    // Lesotho
        [Display(Name = "Lebanon")]
        LBN,    // Lebanon
        [Display(Name = "Russia")]
        RUS,    // Russian Federation
    }
}
