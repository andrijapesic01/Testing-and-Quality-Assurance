import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdatePortfolioComponent } from './update-portfolio.component';

describe('UpdatePortfolioComponent', () => {
  let component: UpdatePortfolioComponent;
  let fixture: ComponentFixture<UpdatePortfolioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdatePortfolioComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UpdatePortfolioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
