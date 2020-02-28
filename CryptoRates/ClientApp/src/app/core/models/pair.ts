import { Currency } from "./currency";

export class Pair {
  pairId: number;
  userId: string;
  firstCurrency: Currency;
  secondCurrency: Currency;
  priceFirstToSecond: number;
  previousPriceFirstToSecond: number;
  targetPrice: number;
  targetPriceAbsoluteChange: number;
  targetPricePercentChange: number;
  isNotifyOnPrice: boolean;
  isNotifyOnAbsoluteChange: boolean;
  isNotifyOnPercentChange: boolean;
}
