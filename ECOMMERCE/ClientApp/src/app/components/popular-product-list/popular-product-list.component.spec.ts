import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopularProductListComponent } from './popular-product-list.component';

describe('PopularProductListComponent', () => {
  let component: PopularProductListComponent;
  let fixture: ComponentFixture<PopularProductListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PopularProductListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PopularProductListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
