import { Http, Response } from "@angular/http"
import { Injectable } from "@angular/core"
//import {IProduct} from './product';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

@Injectable()
export class ProductService {
  myUrl = 'api/getcities';
  myUrl2 = 'api/getweather';

  constructor(private _http: Http) { }

  getProducts(country: string): Observable<any> {
    return this._http.get(this.myUrl + '/' + country)
      .map((response: Response) => <any>response.json())
      //.do((data: any) => console.log(`data: ${JSON.stringify(data)}`))
      .catch(this.handleError);
  }

  getWeather(country: string, city: string): Observable<any> {
    return this._http.get(this.myUrl2 + '/' + country + '/' + city)
      .map((response: Response) => <any>response.json())
      //.do((data: any) => console.log(`data: ${JSON.stringify(data)}`))
      .catch(this.handleError);
  }

  handleError(error: Response) {
    console.log(error);
    return Observable.throw(error.json().error || 'Server error');
  }
}