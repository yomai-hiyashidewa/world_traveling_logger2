// Country codes (ISO 3166-1 alpha-3)
export type CountryType = string;

// Currency codes
export type CurrencyType = string;

export type MajorCurrencyType = 'JPY' | 'USD' | 'EUR';

export const MAJOR_CURRENCIES: MajorCurrencyType[] = ['JPY', 'USD', 'EUR'];

// Accommodation types
export const AccommodationTypes = [
  'ParentsHouse', 'Domitory', 'SingleRoom', 'Airplane', 'Bus', 'Ferry',
  'Train', 'Airport', 'FriendHouse', 'Hotel',
] as const;
export type AccommodationType = typeof AccommodationTypes[number];

export function parseAccommodationType(s: string): AccommodationType {
  const found = AccommodationTypes.find(t => t.toLowerCase() === s.toLowerCase());
  if (!found) throw new Error(`Unknown AccommodationType: ${s}`);
  return found;
}

// Transportation types
export const TransportationTypes = [
  'Train', 'Bus', 'AirPlane', 'Ferry', 'Subway', 'Taxi', 'UBER', 'UBERMoto',
  'Car', 'Tram', 'Bike', 'MotorCycle', 'Tuktuk', 'BycleTaxi', 'Boat',
  'Ropeway', 'Cesna', 'Track', 'Geepny', 'Walking',
  'LocalBus', 'MiddleDistanceBus', 'LongDistanceBus',
  'LocalTrain', 'MiddleDistanceTrain', 'LongDistanceTrain',
] as const;
export type TransportationType = typeof TransportationTypes[number];

export function parseTransportationType(s: string): TransportationType {
  const found = TransportationTypes.find(t => t.toLowerCase() === s.toLowerCase());
  if (!found) throw new Error(`Unknown TransportationType: ${s}`);
  return found;
}

// Place types
export const PlaceTypes = [
  'Station', 'Terminal', 'Inn', 'AirPort', 'Port', 'Central', 'Stop',
  'Beach', 'Suberb', 'Museum', 'Park', 'Heritage', 'Border',
  'Hospital', 'Lake', 'Castle', 'Catedral', 'Palace', 'Church', 'Dep',
] as const;
export type PlaceType = typeof PlaceTypes[number];

export function parsePlaceType(s: string): PlaceType {
  const found = PlaceTypes.find(t => t.toLowerCase() === s.toLowerCase());
  if (!found) throw new Error(`Unknown PlaceType: ${s}`);
  return found;
}

// Sightseeing types
export const SightseeingTypes = [
  'Visiting', 'Trekking', 'Walking', 'Eating', 'KickBoard', 'Cycring',
  'CableCar', 'Tour', 'Boat', 'HotSpring', 'Museum', 'Church',
  'Beach', 'Zoo', 'Heritage', 'Overviewing', 'Waterfall', 'Castle',
  'Nature', 'Canal', 'Park', 'Restaurant', 'Eatery', 'Stand',
  'Bakery', 'BugerShop', 'ChainStore', 'Cafe', 'Bar',
  'Food', 'Drink', 'Snack', 'InnBreakfast', 'InnDinner',
  'Transportfood', 'Tourfood', 'Fesfood', 'Supermarket', 'Sovnir',
] as const;
export type SightseeingType = typeof SightseeingTypes[number];

// Other expense types (also used as sightseeing subtypes that get reclassified)
export const OtherTypes = [
  'Insurance', 'Ticket', 'Accident', 'Event', 'Other', 'Shopping',
  'Medical', 'Washing', 'Tax', 'Exchange', 'Cashing', 'Haircut',
  'Tips', 'PartTimeJob', 'Toilet',
] as const;
export type OtherType = typeof OtherTypes[number];

// All types that can appear in the sightseeing CSV type column
export type SightseeingCsvType = SightseeingType | OtherType;

export function parseSightseeingCsvType(s: string): SightseeingCsvType {
  const all = [...SightseeingTypes, ...OtherTypes] as readonly string[];
  const found = all.find(t => t.toLowerCase() === s.toLowerCase());
  if (!found) throw new Error(`Unknown SightseeingCsvType: ${s}`);
  return found as SightseeingCsvType;
}

export function isOtherType(t: SightseeingCsvType): t is OtherType {
  return (OtherTypes as readonly string[]).includes(t);
}

export function parseOtherType(s: string): OtherType {
  const found = (OtherTypes as readonly string[]).find(t => t.toLowerCase() === s.toLowerCase());
  if (!found) throw new Error(`Unknown OtherType: ${s}`);
  return found as OtherType;
}

// Distance-based reclassification for Train/Bus
const LOCAL_THRESHOLD = 50;
const MIDDLE_THRESHOLD = 200;

export function reclassifyTransportationType(type: TransportationType, distance: number): TransportationType {
  if (type === 'Train') {
    if (distance < LOCAL_THRESHOLD) return 'LocalTrain';
    if (distance < MIDDLE_THRESHOLD) return 'MiddleDistanceTrain';
    return 'LongDistanceTrain';
  }
  if (type === 'Bus') {
    if (distance < LOCAL_THRESHOLD) return 'LocalBus';
    if (distance < MIDDLE_THRESHOLD) return 'MiddleDistanceBus';
    return 'LongDistanceBus';
  }
  return type;
}

// Keyword-based auto-classification for sightseeing context
const KEYWORD_MAP: [string, SightseeingCsvType][] = [
  ['BEACH', 'Beach'], ['MUSEUM', 'Museum'], ['CHURCH', 'Church'],
  ['ZOO', 'Zoo'], ['HERITAGE', 'Heritage'], ['WATERFALL', 'Waterfall'],
  ['CASTLE', 'Castle'], ['PARK', 'Park'], ['RESTAURANT', 'Restaurant'],
  ['CAFE', 'Cafe'], ['BAR', 'Bar'], ['CANAL', 'Canal'],
  ['INSURANCE', 'Insurance'], ['TICKET', 'Ticket'], ['SHOPPING', 'Shopping'],
  ['MEDICAL', 'Medical'], ['WASHING', 'Washing'], ['EXCHANGE', 'Exchange'],
  ['HAIRCUT', 'Haircut'], ['SUPERMARKET', 'Supermarket'],
];

export function classifySightseeingByKeyword(context: string, csvType: SightseeingCsvType): SightseeingCsvType {
  // If the CSV already has a specific type, use it
  if (csvType !== 'Visiting') return csvType;
  const upper = context.toUpperCase();
  for (const [keyword, type] of KEYWORD_MAP) {
    if (upper.includes(keyword)) return type;
  }
  return csvType;
}

// Country code -> Display name mapping (from CountryType.cs)
const COUNTRY_NAME_MAP: Record<string, string> = {
  JPN: 'Japan', UNK: 'No Country',
  // East Asia
  KOR: 'South Korea', TWN: 'Taiwan', HKG: 'Hong Kong',
  PHL: 'Philippines', VNM: 'Viet Nam', MYS: 'Malaysia',
  // Oceania
  AUS: 'Australia',
  // North America
  USA: 'U.S.A', CAN: 'Canada',
  // Central America
  MEX: 'Mexico', GTM: 'Guatemala', BLZ: 'Belize', SLV: 'El Salvador',
  HND: 'Honduras', NIC: 'Nicaragua', CRI: 'Costa Rica', PAN: 'Panama', CUB: 'Cuba',
  // South America
  COL: 'Colombia', ECU: 'Ecuador', PER: 'Pérou', BOL: 'Bolivia',
  CHL: 'Chile', PRY: 'Paraguay', ARG: 'Argentina', URY: 'Uruguay', BRA: 'Brazil',
  // Europe - Islands / UK
  ISL: 'Iceland', IRL: 'Ireland', GBR: 'UK',
  // North Europe
  NOR: 'Norway', SWE: 'Sweden', FIN: 'Finland', DNK: 'Denmark',
  // West Europe
  PRT: 'Portugal', ESP: 'Spain', AND: 'Andorra', FRA: 'France', MCO: 'Monaco',
  BEL: 'Belgium', LUX: 'Luxembourg', NLD: 'Netherlands', DEU: 'Germany',
  CHE: 'Switzerland', LIE: 'Liechtenstein', AUT: 'Austria',
  HUN: 'Hungary', CZE: 'Czechia', SVK: 'Slovakia', POL: 'Poland',
  ITA: 'Italy', MOM: 'Knights of Malta', VAT: 'Vatican City', SMR: 'San Marino',
  // Baltic / Balkan
  EST: 'Estonia', LVA: 'Latvia', LTU: 'Lithuania',
  HRV: 'Croatia', SVN: 'Slovenia', BIH: 'B&H', SRB: 'Serbia',
  KVX: 'Kosovo', MNE: 'Montenegro', MKD: 'North Macedonia',
  BGR: 'Bulgaria', ROU: 'Romania', MDA: 'Moldova', ALB: 'Albania', GRC: 'Greece',
  CYP: 'Cyprus', MLT: 'Malta',
  // North Africa
  EGY: 'Egypt', TUN: 'Tunisia', MAR: 'Morocco',
  // Central Asia / Middle East
  TUR: 'Turkiye', QAT: 'Qatar', ARE: 'UAE', GEO: 'Georgia', ARM: 'Armenia', UZB: 'Uzbekistan',
  // South Asia
  IND: 'India', NCY: 'North Cyprus',
  // Others
  AZE: 'Azerbaijan', AFG: 'Afghanistan', DZA: 'Algeria', ABW: 'Aruba',
  AIA: 'Anguilla', AGO: 'Angola', ATG: 'Antigua and Barbuda',
  YEM: 'Yemen', ISR: 'Israel', IRQ: 'Iraq', IRN: 'Iran', IDN: 'Indonesia',
  WLF: 'Wallis and Futuna', UGA: 'Uganda', UKR: 'Ukraine', SWZ: 'Eswatini',
  ETH: 'Ethiopia', ERI: 'Eritrea', OMN: 'Oman', GHA: 'Ghana', CPV: 'Cabo Verde',
  GUY: 'Guyana', KAZ: 'Kazakhstan', GAB: 'Gabon', CMR: 'Cameroon', GMB: 'Gambia',
  KHM: 'Cambodia', GIN: 'Guinea', GNB: 'Guinea-Bissau', CUW: 'Curaçao',
  KIR: 'Kiribati', KGZ: 'Kyrgyzstan', KWT: 'Kuwait', GRD: 'Grenada', KEN: 'Kenya',
  CIV: "Côte d'Ivoire", COM: 'Comoros', COG: 'Congo', COD: 'Congo, Democratic Republic of the',
  SAU: 'Saudi Arabia', WSM: 'Samoa', ZMB: 'Zambia', SLE: 'Sierra Leone',
  DJI: 'Djibouti', JAM: 'Jamaica', SYR: 'Syria', SGP: 'Singapore',
  ZWE: 'Zimbabwe', SDN: 'Sudan', SUR: 'Suriname', LKA: 'Sri Lanka',
  SYC: 'Seychelles', SEN: 'Senegal', LCA: 'Saint Lucia', SOM: 'Somalia',
  THA: 'Thailand', TJK: 'Tajikistan', TZA: 'Tanzania', TCD: 'Chad',
  CAF: 'Central African Republic', CHN: 'China', PRK: 'North Korea',
  TUV: 'Tuvalu', TGO: 'Togo', TKL: 'Tokelau', DOM: 'Dominican Republic',
  DMA: 'Dominica', TTO: 'Trinidad and Tobago', TKM: 'Turkmenistan', TON: 'Tonga',
  NGA: 'Nigeria', NRU: 'Nauru', NAM: 'Namibia', ATA: 'Antarctica', NIU: 'Niue',
  NER: 'Niger', ESH: 'Western Sahara', NCL: 'New Caledonia', NZL: 'New Zealand',
  NPL: 'Nepal', BHR: 'Bahrain', HTI: 'Haiti', PAK: 'Pakistan', VUT: 'Vanuatu',
  BHS: 'Bahamas', PNG: 'Papua New Guinea', BMU: 'Bermuda', PLW: 'Palau',
  BRB: 'Barbados', PSE: 'Palestine', BGD: 'Bangladesh', TLS: 'Timor-Leste',
  PCN: 'Pitcairn', FJI: 'Fiji', BTN: 'Bhutan', PRI: 'Puerto Rico',
  FRO: 'Faroe Islands', BFA: 'Burkina Faso', BRN: 'Brunei', BDI: 'Burundi',
  BEN: 'Benin', VEN: 'Venezuela', BLR: 'Belarus', BWA: 'Botswana',
  MAC: 'Macau', MDG: 'Madagascar', MYT: 'Mayotte', MWI: 'Malawi', MLI: 'Mali',
  FSM: 'Micronesia', ZAF: 'South Africa', SSD: 'South Sudan', MMR: 'Myanmar',
  MUS: 'Mauritius', MRT: 'Mauritania', MOZ: 'Mozambique', MDV: 'Maldives',
  MNG: 'Mongolia', JOR: 'Jordan', LAO: 'Laos', LBY: 'Libya', LBR: 'Liberia',
  RWA: 'Rwanda', LSO: 'Lesotho', LBN: 'Lebanon', RUS: 'Russia',
};

export function countryName(code: CountryType): string {
  return COUNTRY_NAME_MAP[code] ?? code;
}

// Tab categories for the UI
export type TabType = 'accommodation' | 'transportation' | 'sightseeing' | 'other' | 'summary' | 'route';
