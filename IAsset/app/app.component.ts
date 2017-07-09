import { Component } from '@angular/core';
import { ProductService } from './product.service'

@Component({
  selector: 'my-app',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.css'],
  moduleId: module.id
})

export class AppComponent {
  errorMessage = '';
  citiesData: string[] = [];
  currentSub: any;
  weatherData: any = null;
  loading = false;

  constructor(private _productService: ProductService) { }

  valuechange(newValue: any) {
    this.reset();
    this.citiesData = [];
    if (this.currentSub !== undefined) this.currentSub.unsubscribe();
    if (newValue === '') return;

    this.currentSub = this._productService.getProducts(newValue).subscribe(
      p => {
        if (Object.keys(p).length === 0) this.errorMessage = "The service didn't find a country";
        else this.citiesData = p;

        this.loading = false;
      },
      error => {
        this.errorMessage = `An error occured. Message: ${<any>error}`, this.loading = false;
      });
  }

  selectItem(city: string) {
    this.reset();
    const partsOfStr = city.split(',');
    this.currentSub = this._productService.getWeather(partsOfStr[1], partsOfStr[0]).subscribe(
      p => {
        if (Object.keys(p).length === 0) this.errorMessage = "The service didn't find this city";
        else this.weatherData = p;

        this.loading = false;
      },
      error => { this.errorMessage = `An error occured. Message: ${<any>error}`, this.loading = false; });
  }

  reset() {
    this.errorMessage = '';
    this.loading = true;
    this.weatherData = null;
  }
}