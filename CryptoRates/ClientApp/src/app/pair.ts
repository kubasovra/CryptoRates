export class Pair {
  pairId: number;
  userId: string;
  firstCurrencyName: string;
  firstCurrencySymbol: string;
  firstCurrencyImageUrl: string;
  firstCurrencyPageUrl: string;
  secondCurrencyName: string;
  secondCurrencySymbol: string;
  secondCurrencyImageUrl: string;
  secondCurrencyPageUrl: string;
  priceFirstToSecond: number;
  previousPriceFirstToSecond: number;
  targetPrice: number;
  targetPriceAbsoluteChange: number;
  targetPricePercentChange: number;
  isNotifyOnPrice: boolean;
  isNotifyOnAbsoluteChange: boolean;
  isNotifyOnPercentChange: boolean;
}
