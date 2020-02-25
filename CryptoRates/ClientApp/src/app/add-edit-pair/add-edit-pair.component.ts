import { Component, OnInit, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Currency } from 'src/app/currency';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-add-edit-pair',
  templateUrl: './add-edit-pair.component.html',
  styleUrls: ['./add-edit-pair.component.css']
})
export class AddEditPairComponent implements OnInit {


  public allCurrencies: Currency[];
  myControl = new FormControl();
  filteredCurrencies: Observable<Currency[]>;
  baseUrl: string;

  
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }
  //The "allCurrencies" array remains "undefined"
  ngOnInit() {

    this.http.get<Currency[]>(this.baseUrl + 'currencies').subscribe(result => {
      this.allCurrencies = result;
    }, error => console.error(error));

    this.filteredCurrencies = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => typeof value === 'string' ? value : value.name),
        map(name => name ? this._filter(name) : this.allCurrencies.slice())
    );

  }

  displayFn(currency: Currency): string {
    return currency && currency.name ? currency.name : '';
  }

  private _filter(name: string): Currency[] {
    const filterValue = name.toLowerCase();

    return this.allCurrencies.filter(currency => currency.name.toLowerCase().indexOf(filterValue) === 0);
  }

}
