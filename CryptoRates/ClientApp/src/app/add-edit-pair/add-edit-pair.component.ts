import { Component, OnInit, Inject, Input, EventEmitter, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Currency } from 'src/app/core/models/currency';
import { Pair } from 'src/app/core/models/pair';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { CurrenciesService } from '../core/services/currencies.service';
import { PairsService } from '../core/services/pairs.service';

@Component({
  selector: 'app-add-edit-pair',
  templateUrl: './add-edit-pair.component.html',
  styleUrls: ['./add-edit-pair.component.css']
})
export class AddEditPairComponent implements OnInit {

  @Output() pairAdded: EventEmitter<Pair> = new EventEmitter<Pair>();
  public allCurrencies: Currency[] = [];
  myControl = new FormControl();
  filteredCurrencies: Observable<Currency[]>;
  baseUrl: string;


  constructor(private currenciesService: CurrenciesService, private pairsService: PairsService) { }

  ngOnInit() {
    this.currenciesService.getAllCurrencies().subscribe(result => {
      this.allCurrencies = result;
    }, error => console.error(error));

    this.filteredCurrencies = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => typeof value === 'string' ? value : value.name),
        map(name => name ? this._filter(name) : this.allCurrencies.slice())
    );

  }

  private _filter(name: string): Currency[] {
    const filterValue = name;

    return this.allCurrencies.filter(currency => currency.name.toLowerCase().indexOf(filterValue) === 0);
  }

  private onClickAddPair(firstCurrencyName: string, secondCurrencyName: string, targetPrice: number) {
    let pair = new Pair();
    pair.firstCurrencyName = firstCurrencyName;
    pair.secondCurrencyName = secondCurrencyName;
    pair.targetPrice = Number(targetPrice);
    if (pair.targetPrice != 0) {
      pair.isNotifyOnPrice = true;
    }

    this.pairsService.addPair(pair).subscribe((event) => this.pairAdded.emit(event));
  }
}
