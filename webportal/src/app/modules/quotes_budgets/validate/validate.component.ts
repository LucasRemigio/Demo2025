import { Component, OnInit } from '@angular/core';
import { EMAIL_CATEGORIES, EMAIL_STATUSES } from 'app/modules/filtering/filtering.types';

@Component({
  selector: 'app-validate',
  templateUrl: './validate.component.html',
  styleUrls: ['./validate.component.scss']
})
export class ValidateComponent implements OnInit {
  statusList = EMAIL_STATUSES;
  categoryList = EMAIL_CATEGORIES;
  constructor() { }

  ngOnInit(): void {
  }

}
