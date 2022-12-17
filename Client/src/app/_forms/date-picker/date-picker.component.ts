import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({selector: 'app-date-picker', templateUrl: './date-picker.component.html', styleUrls: ['./date-picker.component.css']})

export class DatePickerComponent implements ControlValueAccessor {
  @Input() label= '';
  @Input() maxDate:Date|undefined;

  //-configuração do date picker do bootstrap, para o css funcionar precisei fazer um import no css global.
  bsConfig:Partial<BsDatepickerConfig>|undefined;

  constructor(@Self() public ngControl:NgControl) {
    this.ngControl.valueAccessor= this;

    this.bsConfig={
      containerClass: "theme-green",
      dateInputFormat: "DD MMMM YYYY",
      isAnimated: true
    }
  }

  get control():FormControl{
    return this.ngControl.control as FormControl;
  }


  writeValue(obj: any): void {}
  registerOnChange(fn: any): void {}
  registerOnTouched(fn: any): void {}
  setDisabledState?(isDisabled: boolean): void {}

  ngOnInit(): void {
  }

}
