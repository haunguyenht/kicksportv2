import { ActivatedRoute } from '@angular/router'
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core'
import { FormArray, FormBuilder, Validators } from '@angular/forms'
import { Store, select } from '@ngrx/store'
import { Subscription } from 'rxjs'

import { AppState } from '../../../core/store/app.state'
import { BaseComponent } from '../../base.component'
import { CategoriesService } from '../../../core/services/categories/categories.service'
import { CategoryModel } from '../models/CategoryModel'
import { IngredientModel } from '../models/IngredientModel'
import { IngredientsService } from '../../../core/services/ingredients/ingredients.service'
import { ProductModel } from '../../products/models/ProductModel'
import { ProductsService } from '../../../core/services/products/products.service'

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.scss']
})
export class EditProductComponent extends BaseComponent implements OnInit {
  protected availableIngredients: IngredientModel[]
  protected categories: CategoryModel[]
  protected editForm
  protected notFoundMessage = 'PRODUCT NOT FOUND'
  protected product: ProductModel
  protected selectedIngredients: IngredientModel[] = []
  private id: string
  private categoriesSubscription$: Subscription
  private ingredientsSubscription$: Subscription
  private productsSubscription$: Subscription

  @ViewChild('labelImport', {static: false}) labelImport: ElementRef;
  fileToUpload: File = null;

  constructor(
    private categoriesService: CategoriesService,
    private fb: FormBuilder,
    private ingredientsService: IngredientsService,
    private productsService: ProductsService,
    private route: ActivatedRoute,
    private store: Store<AppState> ) {
      super()
    }

  ngOnInit() {
    this.categoriesService.getAllCategories()
    this.ingredientsService.getAllIngredients()

    this.id = this.route.snapshot.paramMap.get('id')
    this.productsSubscription$ = this.store
      .pipe(select(state => state.products.all))
      .subscribe(products => {
        this.product = products.find(p => p.id === this.id)
        this.product.ingredients.forEach(ingredient => {
          this.selectedIngredients.push({name: ingredient})
        })
      })

    this.categoriesSubscription$ = this.store
      .pipe(select(state => state.categories.all))
      .subscribe(categories => {
        this.categories = categories
      })

    this.ingredientsSubscription$ = this.store
      .pipe(select(state => state.ingredients.all))
      .subscribe(ingredients => {
        this.availableIngredients = ingredients
      })

    this.subscriptions.push(this.productsSubscription$)
    this.subscriptions.push(this.categoriesSubscription$)
    this.subscriptions.push(this.ingredientsSubscription$)

    this.createForm()
  }

  createForm() {
    if (this.product) {
      this.editForm = this.fb.group({
        name: [this.product.name, [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
        category: [this.product.category],
        ingredients: this.fb.array(this.product.ingredients),
        description: [this.product.description, [Validators.required, Validators.minLength(10), Validators.maxLength(220)]],
        image: [null, [Validators.required]],
        weight: [this.product.weight, [Validators.required, Validators.min(250), Validators.max(800)]],
        price: [this.product.price.toFixed(2), [Validators.required, Validators.min(0.1)]]
      })
    } 
  }

  edit() {
    if (this.editForm.invalid) {
      return
    }
    const productFormData = new FormData();
    productFormData.append('name', this.editForm.get('name').value);
    productFormData.append('category', this.editForm.get('category').value);
    this.ingredients.getRawValue().forEach(e => {
      productFormData.append('ingredients[]', e);
    });
    productFormData.append('description', this.editForm.get('description').value);
    productFormData.append('image', this.editForm.get('image').value);
    productFormData.append('weight', this.editForm.get('weight').value);
    productFormData.append('price', this.editForm.get('price').value);
    
    this.productsService.editProduct(this.product.id, productFormData)
  }

  uploadFile(event: any) {
    const pic = (event.target as HTMLInputElement).files[0];
    this.editForm.patchValue({
      image: pic
    });
    this.editForm.get('image').updateValueAndValidity();
  };

  onFileChange(files: FileList) {
    this.labelImport.nativeElement.innerText = Array.from(files)
      .map(f => f.name)
      .join(', ');
    this.fileToUpload = files.item(0);
  }

  addIngredient(ingredient: string) {
    this.editForm.pristine = false
    this.ingredients.push(this.fb.control(ingredient))
  }

  removeIngredient(ingredient: string) {
    this.editForm.pristine = false
    const ingredientIndex: number = this.ingredients.value.findIndex(i => i === ingredient)
    this.ingredients.removeAt(ingredientIndex)
  }

  get name () {
    return this.editForm.get('name')
  }

  get ingredients () {
    return this.editForm.get('ingredients') as FormArray
  }

  get description () {
    return this.editForm.get('description')
  }

  get image () {
    return this.editForm.get('image')
  }

  get weight () {
    return this.editForm.get('weight')
  }

  get price () {
    return this.editForm.get('price')
  }
}
