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

  
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    http.get<Currency[]>(baseUrl + 'currencies').subscribe(result => {
      this.allCurrencies = result;
    }, error => console.error(error));


  }
  //The "allCurrencies" array seems to be empty after the request in constructor,
  //since this error appears in the DevTools console after you click on "Pairs":
  //"Error: Cannot find a differ supporting object '[object Object]' of type 'object'.NgFor only supports binding to Iterables such as Arrays."
  //No idea why is it like that - if you set a breakpoint on line 24, you can see that both "allCurrencies" and "result" are full of data
  ngOnInit() {

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
