import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductWithCategoryComponent } from './product-with-category.component';

describe('ProductWithCategoryComponent', () => {
  let component: ProductWithCategoryComponent;
  let fixture: ComponentFixture<ProductWithCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductWithCategoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductWithCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
