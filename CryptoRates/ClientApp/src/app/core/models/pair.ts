import { Currency } from "./currency";

export class Pair {
  pairId: number;
  userId: string;
  firstCurrency: Currency;
  secondCurrency: Currency;
  historicalData: number[];
  priceFirstToSecond: number;
  previousPriceFirstToSecond: number;
  targetPrice: number;
  isNotifyOnPrice: boolean;
}
