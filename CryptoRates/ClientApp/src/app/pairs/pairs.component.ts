import { Component, OnInit, Inject } from '@angular/core';
import { Pair } from 'src/app/pair';
import { PairsService } from '../core/services/pairs.service';

@Component({
  selector: 'app-pairs',
  templateUrl: './pairs.component.html',
  styleUrls: ['./pairs.component.css']
})
export class PairsComponent implements OnInit {

  public pairs: Pair[];

  constructor(private pairsService: PairsService) { }

  ngOnInit() {
    this.pairsService.getAllPairs().subscribe(result => {
      this.pairs = result;
    }, error => console.error(error));
  }

}

