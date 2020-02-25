import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-pairs',
  templateUrl: './pairs.component.html',
  styleUrls: ['./pairs.component.css']
})
export class PairsComponent implements OnInit {

  public pairs: Pair[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    http.get<Pair[]>(baseUrl + 'pairs').subscribe(result => {
      this.pairs = result;
    }, error => console.error(error));


  }

  ngOnInit() {
  }

}

interface Pair {
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
