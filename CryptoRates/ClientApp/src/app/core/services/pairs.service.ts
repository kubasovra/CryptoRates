import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pair } from 'src/app/core/models/pair';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PairsService {

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getAllPairs(): Observable<Pair[]> {
    return this.http.get<Pair[]>(this.baseUrl + 'api/pairs');
  }

  addPair(pair: Pair): Observable<Pair> {
    return this.http.post<Pair>(this.baseUrl + 'api/pairs', pair);
  }

  deletePair(id: number): Observable<number> {
    return this.http.delete<number>(this.baseUrl + 'api/pairs/' + id);
  }
}
