# Persistence

* **postgres**: relational database
* **Entity Framework Core**: ORM (Object Relational Mapper) for .NET, used to access the database

## Tables:

### `users`

| Column | Type | Nullable | Default | Constraints |
|---|---|---|---|---|
| `id` | `uuid` | NOT NULL |   | PK |
| `email` | `varchar(255)` | NOT NULL |   | UNIQUE (via Index) |
| `password_hash` | `varchar(128)` | NOT NULL |   |   |
| `role` | `varchar(20)` | NOT NULL |   | kein check constraint, verlasse mich auf die Domain |
| `created_at` | `timestamptz` | NOT NULL |   |   |

**Indexes**
| Name | Type | Columns | Eigenschaften |
|---|---|---|---|
| `PK_users` | PK | `id` |   |
| `ix_users_email` | UNIQUE | `email` | btree, ASC |

---

### `sessions`

| Column | Type | Nullable | Default | Constraints |
|---|---|---|---|---|
| `id` | `uuid` | NOT NULL |   | PK |
| `user_id` | `uuid` | NOT NULL |   | FK -> users.id, ON DELETE CASCADE |
| `token_hash` | `varchar(128)` | NOT NULL |   |   |
| `expires_at` | `timestamptz` | NOT NULL |   |   |
| `last_seen` | `timestamptz` | NOT NULL |   |   |
| `created_at` | `timestamptz` | NOT NULL |   |   |
| `roles` | `text[]` | NOT NULL |   | kein check constraint, verlasse mich auf die Domain |

**Indexes**
| Name | Type | Columns |
|---|---|---|
| `PK_sessions` | PK | `id` |
| `ix_sessions_user_id_created_at` | INDEX | `user_id` ASC, `created_at` ASC |

---

### `transactions`

| Column | Type | Nullable | Default | Constraints |
|---|---|---|---|---|
| `id` | `uuid` | NOT NULL |   | PK |
| `user_id` | `uuid` | NOT NULL |   | FK -> users.id, ON DELETE CASCADE |
| `standing_order_id` | `uuid` | NULL |   | noch nicht implementiert |
| `category_id` | `uuid` | NOT NULL |   | FK -> categories.id, ON DELETE RESTRICT |
| `category_source` | `varchar(20)` | NOT NULL |   |   |
| `standing_order_source` | `varchar(20)` | NULL |   |   |
| `amount` | `numeric(12,2)` | NOT NULL |   | CHECK > 0 |
| `type` | `varchar(20)` | NOT NULL |   | verlasse mich auf die Domain |
| `date` | `date` | NOT NULL |   |   |
| `title` | `varchar(255)` | NOT NULL |   |   |
| `description` | `varchar(500)` | NOT NULL |   |   |
| `counter_party` | `varchar(255)` | NOT NULL |   |   |
| `created_at` | `timestamptz` | NOT NULL |   |   |

**Indexes**
| Name | Type | Columns | Eigenschaften |
|---|---|---|---|
| `PK_transactions` | PK | `id` |   |
| `ix_transactions_user_id` | INDEX | `user_id` ASC |   |
| `ix_transactions_category_id` | INDEX | `category_id` ASC |   |
| `ix_transactions_standing_order_id` | INDEX | `standing_order_id` ASC | Partial: `WHERE standing_order_id IS NOT NULL` |
| `ix_transactions_user_id_date` | INDEX | `user_id` ASC, `date` ASC |   |

---

### `categories`

| Column | Type | Nullable | Default | Constraints |
|---|---|---|---|---|
| `id` | `uuid` | NOT NULL |   | PK |
| `user_id` | `uuid` | NOT NULL |   | FK -> users.id, ON DELETE CASCADE |
| `name` | `varchar(255)` | NOT NULL |   |   |
| `is_default` | `boolean` | NOT NULL |   |   |
| `created_at` | `timestamptz` | NOT NULL |   |   |

**Indexes**
| Name | Type | Columns | Eigenschaften |
|---|---|---|---|
| `PK_categories` | PK | `id` |   |
| `ix_categories_user_id_name` | UNIQUE | `user_id` ASC, `name` ASC |   |
| `ix_categories_user_id_unique_default` | UNIQUE | `user_id` ASC | Partial: `WHERE is_default = true` |

---

### `matching_rules`

| Column | Type | Nullable | Default | Constraints |
|---|---|---|---|---|
| `category_id` | `uuid` | NOT NULL |  | PK (1/2), FK -> categories.id |
| `keyword` | `varchar(255)` | NOT NULL |  | PK (2/2) |

**Indexes**
| Name | Typ | Columns | Eigenschaften |
|---|---|---|---|
| `PK_matching_rules` | Composite PK | `category_id` + `keyword` | |