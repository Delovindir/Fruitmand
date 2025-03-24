export type FruitSoort = 'appel' | 'banaan' | 'aardbei' | 'kiwi' | string;

export interface FruitItem {
  soortFruit: string;
  aankoopDatum: string; // ISO date
  isBiologisch?: boolean;
  soortAppel?: string;
  isBedorven: boolean;
}

export interface MandDetail {
  fruits: FruitItem[];
  totaalAantal: number;
  aantalBedorven: number;
  spoilagePercentage: number;
  isBedorven: boolean;
}
