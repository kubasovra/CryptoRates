import { Component, OnInit } from '@angular/core';
import { Pair } from 'src/app/core/models/pair';
import { PairsService } from '../core/services/pairs.service';
import { interval } from 'rxjs';

@Component({
  selector: 'app-pairs',
  templateUrl: './pairs.component.html',
  styleUrls: ['./pairs.component.css']
})
export class PairsComponent implements OnInit {

  public pairs: Pair[];

  constructor(private pairsService: PairsService) {

    if (Notification.permission !== "granted") {
      Notification.requestPermission();
    }

    this.updatePairs();

    //Prices update in the db every 10 seconds
    let updatePairsSubsription = interval(10000).subscribe(() => {
      this.updatePairs();
      this.checkPrices();
    });
  }

  ngOnInit() { }

  newPairAdded(pair: Pair) {
    this.pairs.push(pair);
  }

  deletePair(id: number) {
    this.pairsService.deletePair(id).subscribe(r => {
      this.pairs.splice(this.pairs.findIndex(p => p.pairId === id), 1);
    }, error => console.error(error));
  }

  updatePairs() {
    this.pairsService.getAllPairs().subscribe(result => {
      this.pairs = result;
    }, error => console.error(error));
  }

  checkPrices() {
    //Yet it is still spamming with notifications
    this.pairs.forEach((pair) => {
      if (pair.isNotifyOnPrice) {
        if (pair.previousPriceFirstToSecond > pair.targetPrice && pair.targetPrice > pair.priceFirstToSecond) {
          //Downwards
          this.notify("Bears are pulling!", pair.firstCurrency.symbol + '/' + pair.secondCurrency.symbol + ' crossed ' + pair.targetPrice + ' downwards');
        }
        else if (pair.previousPriceFirstToSecond < pair.targetPrice && pair.targetPrice < pair.priceFirstToSecond) {
          //Upwards
          this.notify("Bulls are pushing!", pair.firstCurrency.symbol + '/' + pair.secondCurrency.symbol + ' crossed ' + pair.targetPrice + ' upwards');
        }
      }
    });
  }

  notify(title: string, text: string) {
    new Notification(title, { body: text });
  }

}
