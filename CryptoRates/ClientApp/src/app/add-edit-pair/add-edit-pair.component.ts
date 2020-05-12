import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormControl, ValidatorFn, AbstractControl, Validators } from '@angular/forms';
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
  firstCurrencyControl = new FormControl();
  secondCurrencyControl = new FormControl();
  firstFilteredCurrencies: Observable<Currency[]>;
  secondFilteredCurrencies: Observable<Currency[]>;
  baseUrl: string;
  currencyNamesAreSame: boolean;


  constructor(private currenciesService: CurrenciesService, private pairsService: PairsService) { }

  ngOnInit() {
    this.currenciesService.getAllCurrencies().subscribe(result => {
      this.allCurrencies = result;

      this.firstCurrencyControl = new FormControl('', [Validators.required, this.nameValidator(this.allCurrencies)]);
      this.secondCurrencyControl = new FormControl('', [Validators.required, this.nameValidator(this.allCurrencies)]);

      this.firstFilteredCurrencies = this.firstCurrencyControl.valueChanges
        .pipe(
          startWith(''),
          map(value => typeof value === 'string' ? value : value.name),
          map(name => name ? this.filter(name) : this.allCurrencies.slice())
      );

      this.secondFilteredCurrencies = this.secondCurrencyControl.valueChanges
        .pipe(
          startWith(''),
          map(value => typeof value === 'string' ? value : value.name),
          map(name => name ? this.filter(name) : this.allCurrencies.slice())
        );
    }, error => console.error(error));
  }

  private filter(name: string): Currency[] {
    this.currencyNamesAreSame = false;
    const filterValue = name.toLowerCase();

    return this.allCurrencies.filter(currency => currency.name.toLowerCase().indexOf(filterValue) === 0 || currency.symbol.toLowerCase().indexOf(filterValue) === 0);
  }

  private onClickAddPair(firstCurrencyName: string, secondCurrencyName: string, targetPrice: number) {
    let pair: Pair = new Pair();
    pair.firstCurrency = this.allCurrencies.find(c => c.name.toLowerCase() === firstCurrencyName.toLowerCase() || c.symbol.toLowerCase() === firstCurrencyName.toLowerCase());
    pair.secondCurrency = this.allCurrencies.find(c => c.name.toLowerCase() === secondCurrencyName.toLowerCase() || c.symbol.toLowerCase() === secondCurrencyName.toLowerCase());
    
    if (pair.firstCurrency === pair.secondCurrency) {
      this.currencyNamesAreSame = true;
      return;
    }

    pair.targetPrice = Number(targetPrice);
    if (pair.targetPrice != 0) {
      pair.isNotifyOnPrice = true;
    }

    if (pair.firstCurrency !== undefined && pair.secondCurrency !== undefined) {
      this.pairsService.addPair(pair).subscribe((event) => this.pairAdded.emit(event));
    }
  }

  private nameValidator(allCurrencies: Currency[]): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const found = allCurrencies.findIndex(curr => curr.name.toLowerCase() === control.value.toLowerCase() || curr.symbol.toLowerCase() === control.value.toLowerCase());
      return found === -1 ? { 'currencyControl': { value: control.value } } : null;
    }
  }
}
